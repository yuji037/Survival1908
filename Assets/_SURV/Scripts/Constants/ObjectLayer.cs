using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectLayer
{
	Default			= 0,
	TransparentFX	= 1,
	IgnoreRaycast	= 2,

	Water			= 4,
	UI				= 5,

	PostProcessing	= 8,
	Map				= 9,
}
