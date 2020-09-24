using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// このクライアントで動くプレイヤー。1つしか存在しない
/// </summary>
public sealed class LocalPlayer : PartyChara
{
	//=============================================================
	// フィールド
	//=============================================================
	[SerializeField] private float moveSpeed = 1.0f;
	[SerializeField] private float walkLinearDrag = 8f;

	private static LocalPlayer instance;

	private Vector2				input;
	private Vector2				movePad;
	private Vector2				inputTouchMove;
	private SwingWeaponModule	swingWeaponModule;
	private Transform			locatorSwingWeapon;
	private FieldEvent			fieldEventCanProceed = null;
	private bool				requestedNoSave = false;

	//=============================================================
	// プロパティ
	//=============================================================
	public static LocalPlayer Instance { get { return instance; } }

	//=============================================================
	// メソッド
	//=============================================================
	public override void Awake()
	{
		base.Awake();
		instance = this;

		locatorSwingWeapon = transform.Find("Locator_SwingWeapon");
	}

	public override void Begin()
    {
		LoadLevel();
		base.Begin();

		var skillButtons = UIBattle.Instance.UISkillButtons;
		for (int i = 0; i < skillButtons.Count; ++i)
		{
			var attackModuleIndex = i + 1;
			skillButtons[i].Init(attackModuleIndex, Attack);
			skillButtons[i].SetSkillInfo(skillIDs[attackModuleIndex]);
		}

		swingWeaponModule = new SwingWeaponModule(this);

		UIPartyStatus.Instance.RegisterPartyChara(this);
		UIInventry.Instance.RegisterParyChara(this);

		var status = statusModule as PartyCharaStatusModule;
		status.AddModifiedHPEvent(		UIPartyStatus.Instance.UpdateHP		);
		status.AddModifiedMPEvent(		UIPartyStatus.Instance.UpdateMP		);
		status.AddModifiedFoodEvent(	UIPartyStatus.Instance.UpdateFood	);

		LoadStatus();
	}

	public override void Tick()
    {
		base.Tick();

		UpdateInput();
		UpdateAnim();
		UpdateSwingWeapon();

	}

	public override void FixedTick()
	{
		if ( isAttackBindMove )
		{
			rigidbdy2D.velocity = Vector2.zero;
			return;
		}

		if ( IsWincing )
			return;

		if ( swingWeaponModule.IsGrabbingSwingWeapon() )
		{
			var moveVelocity = movePad * 1.6f;
			moveVelocity += swingWeaponModule.GetCentrifugalVelocity();

			rigidbdy2D.velocity = moveVelocity;
		}
		else
		{
			var moveVelocity = movePad * moveSpeed;

			rigidbdy2D.drag = walkLinearDrag;
			rigidbdy2D.velocity = moveVelocity;
		}
	}

	void UpdateInput()
	{
		var hori = Input.GetAxis("Horizontal");
		var vert = Input.GetAxis("Vertical");

#if UNITY_EDITOR
		// 上下移動と左右移動を同時入力した時(WASDキーのみ)、
		// 弾の出る方向とAnimatorが向かせる方向が異なるので仕方なく調整
		if (		vert > 0 ) vert -= 0.01f;
		else if (	vert < 0 ) vert += 0.01f;
#endif

		input = new Vector2(hori, vert);
		if ( input.sqrMagnitude > 1f )
		{
			// 大きさ1を超えない
			input = input.normalized;
		}

		if ( inputTouchMove.sqrMagnitude > input.sqrMagnitude )
		{
			input = inputTouchMove;
			inputTouchMove = Vector2.zero;
		}

		if ( isAttackBindMove )
		{
			movePad = Vector2.zero;
		}
		else
		{
			movePad = input;

		}
	}

	public void UpdateAnim()
	{
		var speed = Mathf.Max(movePad.sqrMagnitude, 0.1f);

		if( movePad.x != 0f || movePad.y != 0f )
		{
			direction = movePad;
		}

		if ( swingWeaponModule.IsGrabbingSwingWeapon() )
		{
			speed = 0.4f;
			direction = swingWeaponModule.GetDirectionToWeapon();
		}
		animator.SetFloat("Horizontal", Direction.x * speed);
		animator.SetFloat("Vertical",	Direction.y * speed);
	}

	private void UpdateSwingWeapon()
	{
		swingWeaponModule.Update(input);
	}

	public void OnPressAction1Button()
	{
		if ( TryProceedFieldEvent() ) { return; }
		if ( TryAccessCraftFacility() ) { return; }
		if ( TryGrabSwingWeapon() ) { return; }

		Attack(0);
	}

	/// <summary>
	/// フィールドイベント開始または進行
	/// </summary>
	/// <returns></returns>
	public bool TryProceedFieldEvent()
	{
		if ( FieldEventManager.Instance.EventCanProceed == null ) { return false; }

		FieldEventManager.Instance.EventCanProceed.Next();
		return true;
	}

	private bool TryGrabSwingWeapon()
	{
		if ( swingWeaponModule.IsGrabbingSwingWeapon() == false
		  && swingWeaponModule.TryGrabSwingWeapon(locatorSwingWeapon) )
		{
			return true;
		}
		else if ( swingWeaponModule.IsGrabbingSwingWeapon() )
		{
			swingWeaponModule.ReleaseSwingWeapon();
			return true;
		}
		return false;
	}

	private bool TryAccessCraftFacility()
	{
		var currentFacility = CraftDataMan.Instance.CurrentFacility;
		if(currentFacility == null) { return false; }

		// 接触してるなら製作ウィンドウへ
		UIIngame.Instance.OnClickSwitchUIButton(2);
		return true;
	}

	public override void ReceiveDamage(AttackInfo attackInfo)
	{
		base.ReceiveDamage(attackInfo);

		SoundManager.Instance.Play("SE_Hit02");
	}

	public void InputTouchMovement(Vector2 vec)
	{
		if ( vec.sqrMagnitude > 1f )
			vec = vec.normalized;

		inputTouchMove = vec;
	}

	private void LoadLevel()
	{
		Debug.Log("Load Player Level");
		var _level = SaveData.GetInt("PlayerLevel", 1);
		statusModule.SetLevel(_level);
	}

	private void LoadStatus()
	{
		var pcStatus = statusModule as PartyCharaStatusModule;
		var _hp = SaveData.GetFloat("PlayerHP", pcStatus.MaxHP);
		var _mp = SaveData.GetFloat("PlayerMP", pcStatus.MaxMP);
		var _food = SaveData.GetFloat("PlayerFood", 50f);
		pcStatus.InitContinueStatus(_hp, _mp, _food);
		EquipmentModule.Load("");
	}

	private void SaveStatus()
	{
		if (requestedNoSave) { return; }
		var pcStatus = statusModule as PartyCharaStatusModule;
		if (pcStatus == null) { return; }
		Debug.Log("Save Status");
		SaveData.SetInt("PlayerLevel", pcStatus.Level);
		SaveData.SetFloat("PlayerHP", pcStatus.HP);
		SaveData.SetFloat("PlayerMP", pcStatus.MP);
		SaveData.SetFloat("PlayerFood", pcStatus.Food);
		EquipmentModule.Save("");
		SaveData.Save();
	}

	public void RequestNoSave()
	{
		requestedNoSave = true;
	}

	private void OnDestroy()
	{
		Debug.Log("OnDestroy Save");
		SaveStatus();
	}

	private void OnApplicationPause(bool pause)
	{
		// アプリケーション起動、一時停止、終了時に呼ばれる
		if (pause)
		{
			Debug.Log("バックグラウンド行った");
			SaveStatus();
		}
		else
		{
			Debug.Log("復帰した");
		}
	}
}
