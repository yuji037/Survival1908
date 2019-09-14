using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPartyDetailMan : CSingletonMonoBehaviour<CPartyDetailMan>
{
    public void SwitchPartyDetail(int index)
    {
        switch (index)
        {
            case 0:
                // HACK: すっごい頭悪そうなやり方
                CInventryMan.   Instance.DispWindow(true);
                CCraftMan.      Instance.DispWindow(false);
                break;
            case 1:
                CInventryMan.   Instance.DispWindow(false);
                CCraftMan.      Instance.DispWindow(true);
                break;
        }
    }
}
