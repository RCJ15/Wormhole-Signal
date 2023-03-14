using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WormholeSignal
{
    /// <summary>
    /// Contains every single <see cref="Signal"/> in the entire project.
    /// </summary>
    public class SignalList : ScriptableObject
    {
        #region Static Obj
        private static SignalList _cachedObj;

        /// <summary>
        /// A singleton like instance of this object which resides in the "Wormhole Signal" <see cref="Resources"/> folder.
        /// </summary>
        public static SignalList Obj
        {
            get
            {
                if (_cachedObj == null)
                {
                    _cachedObj = Resources.Load<SignalList>("Wormhole Signal/Signal List");
                }

                return _cachedObj;
            }
        }
        #endregion

        #region GUID Dictionary
        private static Dictionary<string, Signal> _cachedSignalGuidDictionary = null;
        public static Dictionary<string, Signal> SignalGuidDictionary
        {
            get
            {
                if (_cachedSignalGuidDictionary == null)
                {
                    _cachedSignalGuidDictionary = new Dictionary<string, Signal>();

                    // Add every signal to the cached dictionary
                    foreach (Signal signal in Obj.signals)
                    {
                        // Ignore null entries
                        if (signal == null)
                        {
                            continue;
                        }

                        // Add to the cached dictionary
                        _cachedSignalGuidDictionary.Add(signal.GUID, signal);
                    }
                }

                return _cachedSignalGuidDictionary;
            }
        }
        #endregion

        #region Name Dictionary
        private static Dictionary<string, Signal> _cachedSignalNameDictionary = null;
        public static Dictionary<string, Signal> SignalNameDictionary
        {
            get
            {
                if (_cachedSignalNameDictionary == null)
                {
                    _cachedSignalNameDictionary = new Dictionary<string, Signal>();

                    // Add every signal to the cached dictionary
                    foreach (Signal signal in Obj.signals)
                    {
                        // Ignore null entries
                        if (signal == null)
                        {
                            continue;
                        }

                        // Are there multiple of the same signal in the dictionary?
                        if (_cachedSignalNameDictionary.ContainsKey(signal.name))
                        {
#if UNITY_EDITOR
                            // Write warning
                            Debug.LogWarning($"There are multiple signals called: \"{signal.name}\"! Please ensure that all signals have unique names if you plan on referencing them by name.");
#endif
                            // Ignore this entry and move to the next entry
                            continue;
                        }

                        // Add to the cached dictionary
                        _cachedSignalNameDictionary.Add(signal.name, signal);
                    }
                }

                return _cachedSignalNameDictionary;
            }
        }
        #endregion

#if UNITY_EDITOR
        #region Serialized Object
        private static SerializedObject _cachedSerializedObject;
        public static SerializedObject SerializedObject
        {
            get
            {
                if (_cachedSerializedObject == null)
                {
                    _cachedSerializedObject = new SerializedObject(Obj);
                }

                return _cachedSerializedObject;
            }
        }
        #endregion
#endif

        [SerializeField] [HideInInspector] private List<Signal> signals = new List<Signal>();

        /// <summary>
        /// A list of all <see cref="Signal"/> in the entire project.
        /// </summary>
        public List<Signal> Signals => signals;

#if UNITY_EDITOR
        /// <summary>
        /// Returns if the given <paramref name="signal"/> is in the signals list. <para/>
        /// NOTE: Only use this within the Unity Editor! Usage in builds will result in errors!
        /// </summary>
        public bool ContainsSignal(Signal signal)
        {
            // Reason why "signals.Contains(signal)" is not used here:
            // Unity will act strange sometimes when you use regular methods in the unity editor
            // This is probably not needed at all but this is just used to play it safe

            // Get the main "signals" list property
            SerializedProperty prop = SerializedObject.FindProperty("signals");

            // Loop through all array elements in the property
            // In this loop we are going to check if this Signal is in the list
            int count = prop.arraySize;
            bool containsSignal = false;

            for (int i = 0; i < count; i++)
            {
                // Get array element at the index
                SerializedProperty arrayElement = prop.GetArrayElementAtIndex(i);

                // Object is in array?
                if (arrayElement.objectReferenceValue == signal)
                {
                    // Break out of this loop and set the "containsSignal" bool to true
                    containsSignal = true;
                    break;
                }
            }

            // Return the value
            return containsSignal;
        }

        /// <summary>
        /// Will add the given <paramref name="signal"/> to the signals list. <para/>
        /// NOTE: Only use this within the Unity Editor! Usage in builds will result in errors!
        /// </summary>
        public void AddSignal(Signal signal)
        {
            // Get the main "signals" list property
            SerializedProperty prop = SerializedObject.FindProperty("signals");

            // Loop through all array elements in the property
            // In this loop we are just going to check if we already have this Signal added or if there is a free null spot to use
            int count = prop.arraySize;
            bool addedSignal = false;

            for (int i = 0; i < count; i++)
            {
                // Get array element at the index
                SerializedProperty arrayElement = prop.GetArrayElementAtIndex(i);

                // Object is already in array?
                if (arrayElement.objectReferenceValue == signal)
                {
                    // Return out of this function as the signal is already in the list
                    // No further work required here!
                    return;
                }
                // Element is null or missing?
                else if (arrayElement.objectReferenceValue == null)
                {
                    // Set this array elements objectReferenceValue to the new signal since this 
                    arrayElement.objectReferenceValue = signal;
                    addedSignal = true;
                }
            }

            // Not added signal yet?
            if (!addedSignal)
            {
                // Insert a new array element at the end of the array
                prop.InsertArrayElementAtIndex(count);

                // Set the new array elements objectReferenceValue to the new signal
                prop.GetArrayElementAtIndex(count).objectReferenceValue = signal;
            }

            // Apply changes
            SerializedObject.ApplyModifiedPropertiesWithoutUndo();

            CleanList(SerializedObject);
        }

        /// <summary>
        /// Will remove the given <paramref name="signal"/> from the signals list. <para/>
        /// NOTE: Only use this within the Unity Editor! Usage in builds will result in errors!
        /// </summary>
        public void RemoveSignal(Signal signal)
        {
            // Get the main "signals" list property
            SerializedProperty prop = SerializedObject.FindProperty("signals");

            // Loop through all array elements in the property
            // In this loop we are just going to check if we have this Signal added and remove it from that spot
            int count = prop.arraySize;

            // No entries in list so return
            if (count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                // Get array element at the index
                SerializedProperty arrayElement = prop.GetArrayElementAtIndex(i);

                // Object is in array?
                if (arrayElement.objectReferenceValue == signal)
                {
                    // Break out of this loop and remove the array element
                    prop.DeleteArrayElementAtIndex(i);
                    break;
                }
            }

            // Apply changes
            SerializedObject.ApplyModifiedPropertiesWithoutUndo();

            CleanList(SerializedObject);
        }

        /// <summary>
        /// Will remove all null entries in the signals list. <para/>
        /// NOTE: Only use this within the Unity Editor! Usage in builds will result in errors!
        /// </summary>
        public static void CleanList(SerializedObject obj)
        {
            // Get the main "signals" list property
            SerializedProperty prop = obj.FindProperty("signals");

            // Loop through all array elements in the property
            // In this loop we are just going to remove all null entries
            int count = prop.arraySize;

            // No entries in list so return
            if (count <= 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                // Get array element at the index
                SerializedProperty arrayElement = prop.GetArrayElementAtIndex(i);

                // Object is null?
                if (arrayElement.objectReferenceValue == null)
                {
                    // Remove the array element
                    prop.DeleteArrayElementAtIndex(i);
                }
            }

            // Apply changes
            obj.ApplyModifiedPropertiesWithoutUndo();
        }
#endif
    
        /// <summary>
        /// Returns a <see cref="Signal"/> by GUID using the <see cref="SignalGuidDictionary"/>.
        /// </summary>
        public static Signal GetByGUID(string guid) => SignalGuidDictionary[guid];
        /// <summary>
        /// Returns a <see cref="Signal"/> by Name using the <see cref="SignalNameDictionary"/>.
        /// </summary>
        public static Signal GetByName(string name) => SignalNameDictionary[name];
    }
}
