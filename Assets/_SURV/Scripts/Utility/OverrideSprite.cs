using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OverrideSprite : MonoBehaviour
{
	private SpriteRenderer sr;

	private static int idMainTex = Shader.PropertyToID("_MainTex");
	private MaterialPropertyBlock block;

	[SerializeField]
	private Texture texture = null;
	public Texture overrideTexture
	{
		get { return texture; }
		set
		{
			//texture = value;
			if ( block == null )
			{
				Init();
			}
			block.SetTexture(idMainTex, texture);
		}
	}

	void Awake()
	{
		Init();
		var oldTexture = block.GetTexture(idMainTex);
		overrideTexture = texture;
		var newScale = new Vector3(
			(float)texture.width / oldTexture.width,
			(float)texture.height / oldTexture.height,
			1f);

		//Debug.Log("texture.width : "+ texture.width);
		//Debug.Log("texture : "+ texture.name);
		sr.size = newScale;
		

		//transform.localScale = Vector3.Scale(transform.localScale, newScale);
	}

	void LateUpdate()
	{
		sr.SetPropertyBlock(block);
	}

	void OnValidate()
	{
		overrideTexture = texture;
	}

	void Init()
	{
		block = new MaterialPropertyBlock();
		sr = GetComponent<SpriteRenderer>();
		sr.GetPropertyBlock(block);
	}
}