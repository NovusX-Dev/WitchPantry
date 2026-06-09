#if UNITY_EDITOR

using UnityEngine;

namespace WingmanInspector {
    
    public static class WingmanExtensions {

        public static long GetId(this Object obj) {
            #if UNITY_6000_4_OR_NEWER
                return (long)EntityId.ToULong(obj.GetEntityId());
            #else
                return (long)obj.GetInstanceID();
            #endif
        }

    }
    
}

#endif