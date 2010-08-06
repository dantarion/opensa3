// *********************************************************************
// PLEASE DO NOT REMOVE THIS DISCLAIusMER
//
// WpfPropertyGrid - By Jaime Olivares
// Article site: http://www.codeproject.com/KB/grid/WpfPropertyGrid.aspx
// Author site: www.jaimeolivares.com
// License: Code Project Open License (CPOL)
//
// *********************************************************************

using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Tabuu.UI
{
    /// <summary>
    /// WPF Native PropertyGrid class, taken from Workflow Foundation Designer
    /// </summary>
    public class WpfPropertyGrid : UserControl
    {
        protected readonly MethodInfo _onSelectionChanged;
        protected readonly Grid _inspector;
        protected readonly ModelTreeManager _modelTreeManager = new ModelTreeManager(new EditingContext());

        /// <summary>
        /// Default constructor, creates a hidden designer view and a property inspector
        /// </summary>
        public WpfPropertyGrid()
        {
            var type = typeof(WorkflowDesigner).Assembly.GetType("System.Activities.Presentation.Internal.PropertyEditing.Metadata.PropertyInspectorMetadata");
            type.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public).Invoke(type, new object[0]);

            type = typeof(WorkflowDesigner).Assembly.GetType("System.Activities.Presentation.Internal.PropertyEditing.Resources.PropertyInspectorResources");
            var resources = (ResourceDictionary)type.GetMethod("GetResources", BindingFlags.Static | BindingFlags.Public).Invoke(type, new object[0]);

            type = typeof(WorkflowDesigner).Assembly.GetType("System.Activities.Presentation.Internal.PropertyEditing.PropertyInspector");

            _inspector = (Grid)type.GetConstructor(new Type[0]).Invoke(new object[0]);
            Resources.MergedDictionaries.Add(_inspector.Resources);
            _inspector.Resources.MergedDictionaries.Add(resources);

            _onSelectionChanged = type.GetMethod("OnSelectionChanged");

            Content = _inspector;
        }

        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(WpfPropertyGrid),
            new PropertyMetadata(null, OnSelectedObjectChanged));

        private static void OnSelectedObjectChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var pg = o as WpfPropertyGrid;
            if (pg != null)
            {
                // if we're a collection
                // select all objects in the collection
                // if we're an observable collection, listen for change and update our selection in that case
                // make sure that the grid updates with changes to dependency property properties on individual objects

                var objects = e.NewValue as IEnumerable ?? new[] { e.NewValue };

                if (e.OldValue is INotifyCollectionChanged)
                    ((INotifyCollectionChanged)e.OldValue).CollectionChanged -= pg.PropertyGrid_CollectionChanged;

                if (objects is INotifyCollectionChanged)
                    ((INotifyCollectionChanged)objects).CollectionChanged += pg.PropertyGrid_CollectionChanged;

                if (e.OldValue is IEnumerable)
                    foreach (var old in ((IEnumerable)e.OldValue).OfType<INotifyPropertyChanged>())
                        (old).PropertyChanged -= pg.PropertyGrid_PropertyChanged;

                var mis = new List<ModelItem>();
                foreach (var obj in objects)
                {
                    if (obj is INotifyPropertyChanged)
                        ((INotifyPropertyChanged)obj).PropertyChanged += pg.PropertyGrid_PropertyChanged;
                    pg._modelTreeManager.Load(obj);
                    mis.Add(pg._modelTreeManager.Root);
                }
                pg._onSelectionChanged.Invoke(pg._inspector, new object[] { null }); // clear previous selection
                pg._onSelectionChanged.Invoke(pg._inspector, new object[] { new Selection(mis) });
            }
        }

        protected void PropertyGrid_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSelectedObjectChanged(this, new DependencyPropertyChangedEventArgs(SelectedObjectProperty, GetValue(SelectedObjectProperty), GetValue(SelectedObjectProperty)));
        }

        protected void PropertyGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnSelectedObjectChanged(this, new DependencyPropertyChangedEventArgs(SelectedObjectProperty, GetValue(SelectedObjectProperty), GetValue(SelectedObjectProperty)));
        }

        public object SelectedObject
        {
            get { return GetValue(SelectedObjectProperty); }
            set { SetCurrentValue(SelectedObjectProperty, value); }
        }
    }
}
