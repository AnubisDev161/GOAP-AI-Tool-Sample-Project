using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GOAPGraph
{
    public class GOAPEditorBlackboard : Blackboard
    {
        public override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
        }
    }
}
