#if UNITY_EDITOR

using UnityEditor;

public class EditorTools : EditorWindow
{
    [MenuItem("Foo/Bar", false, 0)]
    static void Example()
    {
        // ...
    }

}

#endif