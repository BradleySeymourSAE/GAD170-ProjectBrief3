using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(PreviewMap))]
public class PreviewMapEditor : Editor {
    public override void OnInspectorGUI(){
        PreviewMap mapPreview = (PreviewMap)target;


        if (DrawDefaultInspector())
        {
            // If map preview auto update is equal to true 
            if (mapPreview.autoUpdate)
            {
                // Draw the map in editor mode 
                mapPreview.DrawPreviewMapInEditor();
            }
        }


        // If the gui button generate map is clicked 
        if (GUILayout.Button("Generate Map"))
        {
            // Draw the map preview in the editor 
            mapPreview.DrawPreviewMapInEditor();
        }
    }
}