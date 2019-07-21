using UnityEditor;

[CustomEditor(typeof(TweenScale))]
public class TweenScaleEditor : TweenBaseEditor
{
    public override void OnInspectorGUI()
    {
        TweenScale tween = target as TweenScale;
        tween.from = EditorGUILayout.Vector3Field("From", tween.from);
        tween.to = EditorGUILayout.Vector3Field("To", tween.to);

        base.OnInspectorGUI();
    }
}