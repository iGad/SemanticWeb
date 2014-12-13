using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MASSemanticWeb;
using MindFusion.Diagramming.Wpf;
using MindFusion.Diagramming.Wpf.Layout;
using Button = System.Windows.Controls.Button;
using Color = System.Drawing.Color;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace SemanticShell
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int TOOLS_COUNT_CONST = 4;
        private const int NON_TOOLS_COUNT_CONST = 3;
        private SemanticWeb _semanticWeb;
        private bool _isDrag, _isCtrlPressed;
        private List<ToolClass> toolSet = new List<ToolClass>(); 
        private enum TreeViewType
        {
            Entity,
            Arc,
            ArcBetweenEntity
        }
        private Color[] toolColors = new Color[]
    {
        Color.FromArgb(99,128,255),
        Color.FromArgb(18,169,255),
        Color.FromArgb(255,96,146),
        Color.FromArgb(156,74,222),
        Color.FromArgb(75,255,222),
        Color.FromArgb(248,106,222),
        Color.FromArgb(99,255,129),
    };

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < ToolBar.Children.Count; i++)
            {
                Button btn = ToolBar.Children[i] as Button;
                if (btn.Name.IndexOf("ToolBtn", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    toolSet.Add(new ToolClass(btn.Name, i));
                }
            }

            _isDrag = false;
            diagram.DefaultShape = Shapes.Rectangle;
            _isCtrlPressed = false;
        }

        private void ImportMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы *.owl|*.owl";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog().Value)
            {
                _semanticWeb = SemanticWeb.Import(openFileDialog.FileName);
                
                if (_semanticWeb == null)
                {
                    MessageBox.Show("Не удалось открыть файл " + openFileDialog.FileName, "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
                foreach (var ent in _semanticWeb.Nodes)
                    ent.PositionChange += EntityOnPositionChange;
                UpdateControls();
            }
        }

        private void EntityOnPositionChange(object sender, EventArgs eventArgs)
        {
            if ((sender as SemanticNode).IsDisplay)
            {
                this.Dispatcher.Invoke(delegate
                    {
                        SemanticNode entity = sender as SemanticNode;
                        DiagramNode shape = FindEntityInDiagram(sender as SemanticNode);
                        if (shape != null)
                        {
                            Rect rect = new Rect(entity.Position.X, entity.Position.Y, entity.Width, entity.Height);
                            shape.SetRect(rect, false);
                        }
                    });
            }
        }


        private void UpdateControls()
        {
            NodesTrv.Items.Clear();
            ArcsBetweenNodesTrv.Items.Clear();
            ArcsTrv.Items.Clear();
            System.Windows.Controls.TreeViewItem item;
            foreach (SemanticNode node in _semanticWeb.Nodes)
            {
                item = new System.Windows.Controls.TreeViewItem();
                item.Header = node.Name;
                item.Tag = "entity|" + node.Id.ToString();
                item.MouseMove += ItemOnMouseMove;
                //item.PreviewMouseDown +=item_PreviewMouseDown;
                NodesTrv.Items.Add(item);
                foreach (var arcs in node.InArcs)
                {
                    item = new System.Windows.Controls.TreeViewItem();
                    item.Header = node.Name + "-" + arcs.Value.Name + " - " + arcs.Key.Name;
                    item.Tag = "arcs|" + node.Id+"|" + arcs.Value.Id.ToString()+"|"+arcs.Key.Id.ToString();
                    item.MouseMove += ItemOnMouseMove;
                    ArcsBetweenNodesTrv.Items.Add(item);
                }
            }
            ClearToolBar();
            for(int i=0;i<_semanticWeb.Arcs.Count;i++)
            {
                item = new System.Windows.Controls.TreeViewItem();
                item.Header = _semanticWeb.Arcs[i].Name;
                item.Tag = _semanticWeb.Arcs[i].Id.ToString();
                ArcsTrv.Items.Add(item);
                AddToolToToolbar(_semanticWeb.Arcs[i].Name,null,toolColors[0]);
                
            }

        }

        private void ClearToolBar()
        {
            for (int i = TOOLS_COUNT_CONST + NON_TOOLS_COUNT_CONST; i < ToolBar.Children.Count; i++)
            {
                ToolBar.Children.RemoveAt(i);
                toolSet.Remove(toolSet.Find(item => item.ToolIndex == i));
            }
        }

        private void AddToolToToolbar(string toolName, System.Drawing.Image image, Color borderColor)
        {
            TextBlock tblk = new TextBlock();
            tblk.TextWrapping = TextWrapping.WrapWithOverflow;
            tblk.Text = toolName;
            Button btn = new Button();
            btn.Width = 75;
            btn.Height = 60;
            System.Windows.Media.Color nColor = System.Windows.Media.Color.FromRgb(255, 255, 255);
            //if(image == null)
            btn.Background = new SolidColorBrush(nColor);
            btn.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(borderColor.R, borderColor.G, borderColor.B));
            btn.BorderThickness = new Thickness(5);
            btn.Content = tblk;
            btn.FontSize = 14;
            btn.Name = toolName + "ToolBtn";
            btn.Click += toolBtn_Click;
            ToolBar.Children.Add(btn);
            toolSet.Add(new ToolClass(btn.Name, ToolBar.Children.Count - 1));
        }

        private void toolBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string toolName = btn.Name.Remove(btn.Name.LastIndexOf("ToolBtn"));
            
            switch(toolName.ToLower())
            {
                case "replace":
                    btn.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    
                    break;
                default:
                    btn.BorderBrush = (btn.BorderBrush as SolidColorBrush).Color.Equals(System.Windows.Media.Color.FromRgb(0, 0, 0)) ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(toolColors[0].R, toolColors[0].G, toolColors[0].B)) : new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    break;
            }
        }

        private void ItemOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed && !_isDrag )
            {
                System.Windows.Controls.TreeViewItem item = (System.Windows.Controls.TreeViewItem)sender;
                DragDrop.DoDragDrop(item, item.Tag, DragDropEffects.Copy);
                _isDrag = true;
            }
        }

#region Работа с графикой

        private void AddArcToDiagram(SemanticNode fromEntity, SemanticNode toEntity, SemanticArc semanticArc)
        {
            var ent1 = FindEntityInDiagram(fromEntity);//diagram.GetNodeAt(new Point(fromEntity.Position.X, fromEntity.Position.Y));
            if (ent1 == null)
                return;
            var ent2 = FindEntityInDiagram(toEntity);//diagram.GetNodeAt(new Point(toEntity.Position.X, toEntity.Position.Y));
            
            if (ent2 == null)
                return;
            var arc = diagram.Factory.CreateDiagramLink(ent1, ent2);
            arc.AllowMoveEnd = false;
            arc.AllowMoveStart = false;
            arc.Name = fromEntity.Name + semanticArc.Name + toEntity.Name;
            arc.Text = semanticArc.Name;
            //var tool = ToolBar.
            arc.Brush =
                new SolidColorBrush(System.Windows.Media.Color.FromRgb(semanticArc.Color.R, semanticArc.Color.G,
                                                                        semanticArc.Color.B));

        }

        private void AddEntityToDiagramByCoordinates(SemanticNode entity, int x, int y)
        {
            if (!entity.IsDisplay)
            {
                entity.Position = new System.Drawing.Point(x, y);
                AddEntityToDiagram(entity);
            }

        }

        public void AddEntityToDiagram(SemanticNode entity)
        {
            if (!entity.IsDisplay)
            {
                if (entity.Position.X < 0 || entity.Position.Y < 0)
                    return;
                entity.IsDisplay = true;
                Rect rect = new Rect(entity.Position.X - entity.Width/2, entity.Position.Y - entity.Height/2,
                                     entity.Width, entity.Height);
                var subject = diagram.Factory.CreateShapeNode(rect);
                //Shapes.RoundRect);
                subject.Text = entity.Name;
                subject.Name ="_"+ entity.Id.ToString();
                foreach (SemanticNode ent in entity.InArcs.Keys)
                {
                    if (ent.IsDisplay)
                        AddArcToDiagram(ent, entity, entity.InArcs[ent]);
                }
                foreach (SemanticNode ent in entity.OutArcs.Keys)
                {
                    if (ent.IsDisplay)
                        AddArcToDiagram(entity, ent, entity.OutArcs[ent]);
                }
            }
        }

        private void AddEntityToDiagram(SemanticNode entity, object sender, DragEventArgs e)
        {
            entity.Position = new System.Drawing.Point((int)Math.Round(e.GetPosition(sender as Diagram).X),
                                                      (int)Math.Round(e.GetPosition(sender as Diagram).Y));
            AddEntityToDiagram(entity);
        }

        private DiagramNode FindEntityInDiagram(SemanticNode entity)
        {
            try
            {
                for (int i = 0; i < diagram.Items.Count; i++)
                {
                    if (string.Compare(diagram.Items[i].Name, "_"+entity.Id.ToString(), false) == 0)
                    {
                        return diagram.Items[i] as DiagramNode;
                    }
                }
                return null;
                //ShapeNode result = (ShapeNode)diagram.FindName(entity.Name.Replace('#','s'));
                //return result;
            }
            catch (Exception)
            {

                return null;
            }
            
            
        }
        

        private void diagram_Drop(object sender, DragEventArgs e)
        {
            _isDrag = false;
            string[] tempStr = ((string) e.Data.GetData(DataFormats.StringFormat)).Split('|');
            Rect rect;
            if (tempStr[0].ToLower() == "entity")
            {
                int position = Convert.ToInt32(tempStr[1]);
                
                SemanticNode node = _semanticWeb.Nodes[position];
                AddEntityToDiagram(node, sender, e);
            }
            if (tempStr[0].ToLower() == "arcs")
            {
                int entity1Id = Convert.ToInt32(tempStr[1]),entity2Id = Convert.ToInt32(tempStr[3]),arcId = Convert.ToInt32(tempStr[2]);
                SemanticArc semanticArc = _semanticWeb.Arcs[arcId];
                SemanticNode node = _semanticWeb[entity1Id];
                if (!node.IsDisplay)
                    AddEntityToDiagram(node, sender, e);
                node = _semanticWeb[entity2Id];
                if (!node.IsDisplay)
                    AddEntityToDiagram(node, sender, e);
            }
        }

        private void window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrag)
                _isDrag = false;
        }

        private void diagram_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (_isCtrlPressed)
            {
                if (e.Delta > 0)
                    diagram.ZoomFactor -= 0.5;
                else
                    diagram.ZoomFactor += 0.5;
                
            }
        }

        private void diagram_DoubleClicked(object sender, DiagramEventArgs e)
        {
            AddEntity window = new AddEntity();
            if (window.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SemanticNode node = _semanticWeb.AddNode(window.EntityName, window.Description, new System.Drawing.Point((int)Math.Round(e.MousePosition.X),(int)Math.Round(e.MousePosition.Y)));
                UpdateControls();
                Rect rect = new Rect(node.Position.X - node.Width / 2, node.Position.Y - node.Height / 2, node.Width, node.Height);
                var subject = diagram.Factory.CreateShapeNode(rect);
                //Shapes.RoundRect);
                subject.Text = node.Name;
            }
            
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            _isCtrlPressed = (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl);
            ScrolVwr.CanContentScroll = !_isCtrlPressed;
            
        }

        private void window_KeyUp(object sender, KeyEventArgs e)
        {
            _isCtrlPressed = _isCtrlPressed && (e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl);
            ScrolVwr.CanContentScroll = !_isCtrlPressed;
        }

        private void ScrollViewer_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            
        }

        private void diagram_NodeTextEdited(object sender, EditNodeTextEventArgs e)
        {
            if (_semanticWeb.Nodes.Exists(node => node.Name.ToLower() == e.NewText.Trim().ToLower()))
            {
                (sender as ShapeNode).Text = e.OldText;
                return;
            }
            
            var entities = _semanticWeb[(sender as ShapeNode).Name];
            if (entities != null)
            {
                SemanticNode entity = entities[0];
                (sender as ShapeNode).Name = e.NewText;
                entity.Name = e.NewText;
                UpdateControls();
            }
        }

        private void diagram_NodeDeleted(object sender, NodeEventArgs e)
        {
            var entities = _semanticWeb[(sender as ShapeNode).Name];
            if (entities != null)
            {
                entities[0].IsDisplay = false;
            }
        }

        private void diagram_LinkDeleting(object sender, LinkValidationEventArgs e)
        {
            if (
                MessageBox.Show("Вы уверены, что хотите удалить эту связь?", "", MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                
            }
            else
                e.Cancel = true;
            
        }

        private void diagram_NodeDeleting(object sender, NodeValidationEventArgs e)
        {

        }

        private void ViewAllBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var ent in _semanticWeb.Nodes)
            {
                AddEntityToDiagramByCoordinates(ent, Convert.ToInt32(diagram.ActualWidth / 2),
                                                        Convert.ToInt32(diagram.ActualHeight / 2));
            }
            _semanticWeb.Arrange(Convert.ToInt32(diagram.ActualWidth), Convert.ToInt32(diagram.ActualHeight));
        }
#endregion

        private void EntityToolBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEntity window = new AddEntity();
            if (window.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _semanticWeb.AddNode(window.Name,window.Description,new System.Drawing.Point(-1,-1));
                UpdateControls();
            }
        }

        private dynamic GetSelectedTreeViewItem()
        {
            int maxItems =
                (new List<int>() {NodesTrv.Items.Count, ArcsTrv.Items.Count, ArcsBetweenNodesTrv.Items.Count}).Max();
            for (int i = 0; i < maxItems; i++)
            {
                if (i < NodesTrv.Items.Count && ((System.Windows.Controls.TreeViewItem) NodesTrv.Items[i]).IsSelected)
                    return new {id = i, Type = TreeViewType.Entity};
                if(i<ArcsTrv.Items.Count && ((System.Windows.Controls.TreeViewItem)ArcsTrv.Items[i]).IsSelected)
                    return new { id = i, Type = TreeViewType.Arc };
                if (i < ArcsBetweenNodesTrv.Items.Count && ((System.Windows.Controls.TreeViewItem)ArcsBetweenNodesTrv.Items[i]).IsSelected)
                    return new { id = i, Type = TreeViewType.ArcBetweenEntity };
            }
            return new {id = -1, Type = TreeViewType.Arc};
        }

        private void AddBtn_OnClick(object sender, RoutedEventArgs e)
        {
            dynamic thing = GetSelectedTreeViewItem();
            if (thing.id < 0)
                return;
            switch ((TreeViewType)thing.Type)
            {
                case TreeViewType.Entity:
                    AddEntity windowEnt = new AddEntity();
                    if (windowEnt.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _semanticWeb.AddNode(windowEnt.Name,windowEnt.Description,new System.Drawing.Point(-1,-1));
                        UpdateControls();
                    }
                    break;
                case TreeViewType.Arc:
                    AddArc windowArc = new AddArc();
                    if (windowArc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _semanticWeb.AddNode(windowArc.Name,windowArc.Comment,new System.Drawing.Point(-1,-1));
                        UpdateControls();
                    }
                break;
                case TreeViewType.ArcBetweenEntity:
                    AddArcBetweenNodes windowAabn = new AddArcBetweenNodes(_semanticWeb);
                    if (windowAabn.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _semanticWeb.AddArcBetweenNodes(_semanticWeb.Nodes[windowAabn.FromNodeIndex],_semanticWeb.Nodes[windowAabn.ToNodeIndex],
                            _semanticWeb.Arcs[windowAabn.ArcIndex],windowAabn.BothSideArc);
                        UpdateControls();
                    }
                break;
            }
        
        }

        private void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void EditBtn_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void ScrollViewer_ScrollChanged_1(object sender, ScrollChangedEventArgs e)
        {
            //e.Handled = _isCtrlPressed;
        }


        

    }
}
