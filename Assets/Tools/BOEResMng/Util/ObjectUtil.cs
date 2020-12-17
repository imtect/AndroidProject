using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BOE.BOEComponent.Util
{
    public static class ObjectUtil
    {
        public static void ActiveGameobject(GameObject go, bool active)
        {
            if (go.activeSelf != active)
            {
                go.SetActive(active);
            }
        }
    }
}
