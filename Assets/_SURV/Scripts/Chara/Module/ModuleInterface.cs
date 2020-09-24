using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IModuleBegin
{
	void Begin();
}

public interface IModuleTick
{
	void Tick();
}

public interface IModuleLateTick
{
	void LateTick();
}
