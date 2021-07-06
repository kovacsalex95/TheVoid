using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace lxkvcs.PlanetGen
{
    [CustomEditor(typeof(Planet))]
    public class PlanetV3Editor : Editor
    {

        Planet planet;
        Editor surfaceEditor;
        Editor textureEditor;

        public override void OnInspectorGUI()
        {
            GUILayout.Space(16);

            if (GUILayout.Button(string.Format("Preview mode: {0}", planet.previewMode ? "ON" : "OFF")))
            {
                planet.previewMode = !planet.previewMode;
                planet.PropertyChanged();
            }

            if (!planet.previewMode) {
                if (GUILayout.Button("Generate Planet", GUILayout.Height(32)))
                    planet.GeneratePlanet();
            }

            GUILayout.Space(8);

            if (GUILayout.Button("Randomize Planet")) {
                planet.Seed = Mathf.Round(Random.Range(0, 40000f)) / 100f - 200f;
                planet.GeneratePlanet();
            }

            GUILayout.Space(16);

            if (GUILayout.Button("Refresh Material"))
            {
                planet.RefreshFaceMaterials();
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (check.changed)
                    planet.PropertyChanged();
            }

            GUILayout.Space(32);

            DrawSettingsEditor(planet.surfaceSettings, planet.PropertyChanged, ref planet.surfaceSettingsFoldout, ref surfaceEditor);
        }

        void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
        {
            if (settings != null)
            {
                foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    if (foldout)
                    {
                        CreateCachedEditor(settings, null, ref editor);
                        editor.OnInspectorGUI();

                        if (check.changed)
                        {
                            if (onSettingsUpdated != null)
                            {
                                onSettingsUpdated();
                            }
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            planet = (Planet)target;
        }
    }
}