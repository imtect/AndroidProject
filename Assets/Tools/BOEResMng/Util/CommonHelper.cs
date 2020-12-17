using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace BOE.BOEComponent.Util {
    public static class CommonHelper {


        public static IEnumerator Delay(float delaytime, System.Action callback) {
            yield return new WaitForSeconds(delaytime);
            if (callback != null) {
                callback();
            }
        }



        public static void ActiveGameobject(GameObject go, bool active) {
            if (go.activeSelf != active) {
                go.SetActive(active);
            }
        }


        /// <summary>
        /// Ëæ»úÅÅÐò
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> inputList) {

            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);


            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);


            List<T> outputList = new List<T>();
            System.Random rd = new System.Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0) {

                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];


                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
        /// <summary>
        /// Éî¿½±´ls
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object Clone(object obj) {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            memoryStream.Position = 0;
            return formatter.Deserialize(memoryStream);
        }

        public static void SetActiveMeshRenderers(this GameObject target, bool isShow, bool isIncludeChild = true) {
            if (target == null) return;
            var meshRender = target.GetComponent<MeshRenderer>();
            if (meshRender != null)
                meshRender.enabled = isShow;
            if (isIncludeChild) {
                for (int i = 0; i < target.transform.childCount; i++) {
                    var child = target.transform.GetChild(i);
                    var childRenderer = child.GetComponent<MeshRenderer>();
                    if (childRenderer != null)
                        childRenderer.enabled = isShow;
                    SetActiveMeshRenderers(child.gameObject, isShow, isIncludeChild);
                }
            }
        }
    }
}