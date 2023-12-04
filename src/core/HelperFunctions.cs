using Godot;
using System;

namespace Isometric3DEngine
{
    public static class HelperFunctions
    {
        public static T GetParentOfType<T>(Node node)
            where T : Node
        {
            Node parent = node.GetParent();
            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = parent.GetParent();
            }
            return null;
        }
    }
}
