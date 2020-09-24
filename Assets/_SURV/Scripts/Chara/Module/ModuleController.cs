using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController
{
	private List<IModuleBegin>		moduleBeginList		= new List<IModuleBegin>();
	private List<IModuleTick>		moduleTickList		= new List<IModuleTick>();
	private List<IModuleLateTick>	moduleLateTickList	= new List<IModuleLateTick>();

	public void AddModule(Object module)
	{
		if(module is IModuleBegin		mb ) { moduleBeginList.Add(mb); }
		if(module is IModuleTick		mt ) { moduleTickList.Add(mt); }
		if(module is IModuleLateTick	ml ) { moduleLateTickList.Add(ml); }
	}

	public void Begin()
	{
		foreach(var module in moduleBeginList )
		{
			module.Begin();
		}
	}
	public void Tick()
	{
		foreach ( var module in moduleTickList )
		{
			module.Tick();
		}
	}
	public void LateTick()
	{
		foreach ( var module in moduleLateTickList )
		{
			module.LateTick();
		}
	}
}
