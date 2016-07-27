﻿// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Sarif.Viewer
{
    /// <summary>
    ///  The TreeViewHelper class enables binding to the SelectedItem of a TreeView.
    /// </summary>
    /// <remarks>
    /// Original source code taken from http://stackoverflow.com/questions/7153813/wpf-mvvm-treeview-selecteditem.
    /// </remarks>
    public static class TreeViewHelper
    {
        private static Dictionary<DependencyObject, TreeViewSelectedItemBehavior> behaviors = new Dictionary<DependencyObject, TreeViewSelectedItemBehavior>();

        public static object GetSelectedItem(DependencyObject obj)
        {
            return (object)obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(TreeViewHelper), new UIPropertyMetadata(new object(), SelectedItemChanged));

        private static void SelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is System.Windows.Controls.TreeView))
                return;

            if (!behaviors.ContainsKey(obj))
                behaviors.Add(obj, new TreeViewSelectedItemBehavior(obj as System.Windows.Controls.TreeView));

            TreeViewSelectedItemBehavior view = behaviors[obj];
            view.ChangeSelectedItem(e.NewValue);
        }

        private class TreeViewSelectedItemBehavior
        {
            System.Windows.Controls.TreeView _view;
            public TreeViewSelectedItemBehavior(System.Windows.Controls.TreeView view)
            {
                _view = view;
                view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
            }

            internal void ChangeSelectedItem(object p)
            {
                // BUGBUG: This will only work for root nodes in the tree.
                //         To make this work for nodes deeper in the tree, you need to walk the tree and call TreeViewItem.ItemContainerGenerator
                //         at each level of the tree.
                TreeViewItem item = (TreeViewItem)_view.ItemContainerGenerator.ContainerFromItem(p);

                if (item != null)
                {
                    item.IsSelected = true;
                }
            }
        }
    }
}
