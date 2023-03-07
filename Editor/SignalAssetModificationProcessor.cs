using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WormholeSignal.Editor
{
    /// <summary>
    /// Handles the asset modification of any <see cref="Signal"/> asset.
    /// </summary>
    public class SignalAssetModificationProcessor : AssetModificationProcessor
    {
        /// <summary>
        /// Calle whenever any asset is going to be deleted.
        /// </summary>
        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
        {
            // Try to load the asset at the path as a Signal asset
            Signal signal = AssetDatabase.LoadMainAssetAtPath(path) as Signal;

            // Check if the conversion to a Signal asset was successful, meaning that we are trying to delete a Signal Asset
            if (signal != null)
            {
                // If so, then we remove this signal from the list
                SignalList.Obj.RemoveSignal(signal);
            }

            // Return that we did not handle the deletion of the file
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
