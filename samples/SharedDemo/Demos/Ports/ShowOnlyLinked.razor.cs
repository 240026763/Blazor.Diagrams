using Blazor.Diagrams;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;

namespace SharedDemo.Demos.Ports
{
    public partial class ShowOnlyLinked : ComponentBase
    {
        public readonly BlazorDiagram BlazorDiagram = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            LayoutData.Title = "Port Renderer";
            LayoutData.Info = "When locked, only render ports that have links.";
            LayoutData.DataChanged();

            BlazorDiagram.RegisterComponent<NodeModel, OnlyLinkedPortNodeWidget>();

            var node1 = NewNode(50, 50);
            var node2 = NewNode(300, 300);
            var node3 = NewNode(300, 50);
            BlazorDiagram.Nodes.Add(new[] { node1, node2, node3 });

            BlazorDiagram.Links.Add(new LinkModel(node1.GetPort(PortAlignment.Right), node2.GetPort(PortAlignment.Left))
            {
                SourceMarker = LinkMarker.Arrow,
                TargetMarker = LinkMarker.Arrow
            });
            BlazorDiagram.Links.Add(new LinkModel(node2.GetPort(PortAlignment.Right), node3.GetPort(PortAlignment.Right))
            {
                Router = Routers.Orthogonal,
                PathGenerator = PathGenerators.Straight,
                SourceMarker = LinkMarker.Arrow,
                TargetMarker = LinkMarker.Arrow
            });
        }

        private bool _allLocked = false;
        public bool AllLocked
        {
            get => _allLocked;
            set
            {
                _allLocked = value;
                foreach (var link in BlazorDiagram.Links)
                    link.Locked = _allLocked;
                foreach (var node in BlazorDiagram.Nodes)
                {
                    node.Locked = _allLocked;
                    node.RefreshAll();
                }
            }
        }

        private NodeModel NewNode(double x, double y)
        {
            var node = new NodeModel(new Point(x, y));
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Left);
            node.AddPort(PortAlignment.Right);
            return node;
        }
    }
}
