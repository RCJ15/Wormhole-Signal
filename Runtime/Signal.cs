using System;
using System.Collections.Generic;
using UnityEngine;

namespace WormholeSignal
{
    /// <summary>
    /// A single Signal in Wormhole Signal.
    /// </summary>
    [CreateAssetMenu(menuName = "Wormhole Signal", fileName = "New Wormhole Signal", order = 150)]
    public class Signal : ScriptableObject
    {
        [SerializeField] private string _guid = null;
        /// <summary>
        /// The Globally Unique IDentifier (GUID) for this <see cref="Signal"/>.
        /// </summary>
        public string GUID => _guid;

        #region Delegates & Dictionaries
        /// <summary>
        /// OnReceive is called whenever a regular call without parameters is performed on this <see cref="Signal"/>.
        /// </summary>
        private Action OnReceiveCall;
        /// <summary>
        /// OnReceiveAnyCall is called whenever any callback is performed on this <see cref="Signal"/> regardless of what event it is. The input parameter will be null if the callback is a void call.
        /// </summary>
        private Action<object> OnReceiveAnyCall;
        /// <summary>
        /// OnReceiveAnyValue is called whenever a call is given any value at all on this <see cref="Signal"/>.
        /// </summary>
        private Action<object> OnReceiveAnyValue;

        /// <summary>
        /// The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        private readonly Dictionary<Type, Action<object>> ValueCallbackDictionary = new Dictionary<Type, Action<object>>();

        private readonly Dictionary<object, Action<object>> _getValueActionDictionary = new Dictionary<object, Action<object>>();
        private readonly Dictionary<object, Action<object>> _getAnyCallValueActionDictionary = new Dictionary<object, Action<object>>();

        private Action<object> GetValueAction<T>(Action<T> callback, bool unsubscribe = false)
        {
            if (!_getValueActionDictionary.ContainsKey(callback))
            {
                _getValueActionDictionary[callback] = (value) => callback?.Invoke((T)value);

            }
            else if (unsubscribe)
            {
                Action<object> returnCallback = _getValueActionDictionary[callback];

                _getValueActionDictionary.Remove(callback);

                return returnCallback;
            }

            return _getValueActionDictionary[callback];
        }
        private Action<object> GetAnyCallValueAction(Action callback, bool unsubscribe = false)
        {
            if (!_getAnyCallValueActionDictionary.ContainsKey(callback))
            {
                _getAnyCallValueActionDictionary[callback] = (_) => callback?.Invoke();
            }
            else if (unsubscribe)
            {
                Action<object> returnCallback = _getAnyCallValueActionDictionary[callback];

                _getAnyCallValueActionDictionary.Remove(callback);

                return returnCallback;
            }

            return _getAnyCallValueActionDictionary[callback];
        }
        #endregion

        #region Subscribe Methods
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the <see cref="OnReceiveCall"/> event. <para/> OnReceive is called whenever a regular call without parameters is performed on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe(Action callback) => OnReceiveCall += callback;

        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the <see cref="OnReceiveAnyCall"/> event. <para/> OnReceiveAnyCall is called whenever any callback is performed on this <see cref="Signal"/> regardless of what event it is. The input parameter will be null if the callback is a void call.
        /// </summary>
        public void SubscribeAnyCall(Action callback) => OnReceiveAnyCall += GetAnyCallValueAction(callback);
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the <see cref="OnReceiveAnyCall"/> event. <para/> OnReceiveAnyCall is called whenever any callback is performed on this <see cref="Signal"/> regardless of what event it is. The input parameter will be null if the callback is a void call.
        /// </summary>
        public void SubscribeAnyCall(Action<object> callback) => OnReceiveAnyCall += callback;

        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the <see cref="OnReceiveAnyValue"/> event. <para/> OnReceiveAnyValue is called whenever a call is given any value at all on this <see cref="Signal"/>.
        /// </summary>
        public void SubscribeAnyValue(Action callback) => OnReceiveAnyValue += GetAnyCallValueAction(callback);
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the <see cref="OnReceiveAnyValue"/> event. <para/> OnReceiveAnyValue is called whenever a call is given any value at all on this <see cref="Signal"/>.
        /// </summary>
        public void SubscribeAnyValue(Action<object> callback) => OnReceiveAnyValue += callback;

        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the given <paramref name="type"/> <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe(Action<object> callback, Type type)
        {
            if (!ValueCallbackDictionary.ContainsKey(type))
            {
                ValueCallbackDictionary.Add(type, null);
            }

            ValueCallbackDictionary[type] += callback;
        }

        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the type of <typeparamref name="T"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe<T>(Action<T> callback) => Subscribe(GetValueAction(callback), typeof(T));

        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="int"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe(Action<int> callback) => Subscribe<int>(callback);
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="float"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe(Action<float> callback) => Subscribe<float>(callback);
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="bool"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe(Action<bool> callback) => Subscribe<bool>(callback);
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="string"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe(Action<string> callback) => Subscribe<string>(callback);
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="object"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe(Action<object> callback) => Subscribe<object>(callback);
        /// <summary>
        /// Subscribes the given <paramref name="callback"/> to the event in the <see cref="ValueCallbackDictionary"/> with the type of <typeparamref name="T"/> without receiving the value in the method itself. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Subscribe<T>(Action callback) => Subscribe<object>(GetAnyCallValueAction(callback));
        #endregion

        #region Unsubscribe Methods
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the <see cref="OnReceiveCall"/> event. <para/> OnReceive is called whenever a regular call without parameters is performed on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe(Action callback) => OnReceiveCall -= callback;

        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the <see cref="OnReceiveAnyCall"/> event. <para/> OnReceiveAnyCall is called whenever any callback is performed on this <see cref="Signal"/> regardless of what event it is. The input parameter will be null if the callback is a void call.
        /// </summary>
        public void UnsubscribeAnyCall(Action callback) => OnReceiveAnyCall -= GetAnyCallValueAction(callback, true);
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the <see cref="OnReceiveAnyCall"/> event. <para/> OnReceiveAnyCall is called whenever any callback is performed on this <see cref="Signal"/> regardless of what event it is. The input parameter will be null if the callback is a void call.
        /// </summary>
        public void UnsubscribeAnyCall(Action<object> callback) => OnReceiveAnyCall -= callback;

        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the <see cref="OnReceiveAnyValue"/> event. <para/> OnReceiveAnyValue is called whenever a call is given any value at all on this <see cref="Signal"/>.
        /// </summary>
        public void UnsubscribeAnyValue(Action callback) => OnReceiveAnyValue -= GetAnyCallValueAction(callback, true);
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the <see cref="OnReceiveAnyValue"/> event. <para/> OnReceiveAnyValue is called whenever a call is given any value at all on this <see cref="Signal"/>.
        /// </summary>
        public void UnsubscribeAnyValue(Action<object> callback) => OnReceiveAnyValue -= callback;

        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the given <paramref name="type"/> <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe(Action<object> callback, Type type)
        {
            if (!ValueCallbackDictionary.ContainsKey(type))
            {
                return;
            }

            ValueCallbackDictionary[type] -= callback;
        }

        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the type of <typeparamref name="T"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe<T>(Action<T> callback) => Unsubscribe(GetValueAction(callback, true), typeof(T));

        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="int"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe(Action<int> callback) => Unsubscribe<int>(callback);
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="float"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe(Action<float> callback) => Unsubscribe<float>(callback);
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="bool"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe(Action<bool> callback) => Unsubscribe<bool>(callback);
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="string"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe(Action<string> callback) => Unsubscribe<string>(callback);
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the type of <see cref="object"/>. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe(Action<object> callback) => Unsubscribe<object>(callback);
        /// <summary>
        /// Unsubscribes the given <paramref name="callback"/> from the event in the <see cref="ValueCallbackDictionary"/> with the type of <typeparamref name="T"/> without receiving the value in the method itself. <para/> The ValueCallbackDictionary has a collection of all callbacks that receive specific data types on this <see cref="Signal"/>.
        /// </summary>
        public void Unsubscribe<T>(Action callback) => Unsubscribe<object>(GetAnyCallValueAction(callback, true));
        #endregion

        #region Call Methods
        /// <summary>
        /// Calls the specific event that receives no value at all.
        /// </summary>
        public void Call()
        {
            OnReceiveCall?.Invoke();

            OnReceiveAnyCall?.Invoke(null);
        }

        /// <summary>
        /// Calls the specific event that receives a value of type <see cref="object"/>.
        /// </summary>
        public void Call(object value) => Call(value, value.GetType());

        /// <summary>
        /// Calls the specific event that receives a value of type <see cref="object"/>.
        /// </summary>
        public void Call(object value, Type type)
        {
            OnReceiveAnyCall?.Invoke(value);
            OnReceiveAnyValue?.Invoke(value);

            // Return if there is no value callback that has the correct type
            if (!ValueCallbackDictionary.ContainsKey(type))
            {
                return;
            }

            // Call the value callback
            ValueCallbackDictionary[type]?.Invoke(value);
        }

        /// <summary>
        /// Calls the specific event that receives a value of type <typeparamref name="T"/>.
        /// </summary>
        public void Call<T>(T value) => Call(value, typeof(T));
        #endregion

        #region Get Methods
        /// <summary>
        /// Returns a <see cref="Signal"/> by GUID using the <see cref="SignalList.GetByGUID(string)"/> method.
        /// </summary>
        public static Signal GetByGUID(string guid) =>  SignalList.GetByGUID(guid);
        /// <summary>
        /// Returns a <see cref="Signal"/> by Name using the <see cref="SignalList.GetByName(string)"/> method.
        /// </summary>
        public static Signal GetByName(string name) => SignalList.GetByName(name);
        #endregion
    }
}