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
    ///   WPF Native PropertyGrid class, taken from Workflow Foundation Designer
    /// </summary>
    public class WpfPropertyGrid : UserControl
    {
        protected readonly MethodInfo OnSelectionChanged;
        protected readonly Grid Inspector;
        protected readonly ModelTreeManager ModelTreeManager = new ModelTreeManager(new EditingContext());

        /// <summary>
        ///   Default constructor, creates a hidden designer view and a property inspector
        /// </summary>
        public WpfPropertyGrid()
        {
            var type =
                typeof(WorkflowDesigner).Assembly.GetType(
                    "System.Activities.Presentation.Internal.PropertyEditing.Metadata.PropertyInspectorMetadata");
            type.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public).Invoke(type, new object[0]);
            type =
                typeof(WorkflowDesigner).Assembly.GetType(
                    "System.Activities.Presentation.Internal.PropertyEditing.Resources.PropertyInspectorResources");
            var resources =
                (ResourceDictionary)
                type.GetMethod("GetResources", BindingFlags.Static | BindingFlags.Public).Invoke(type, new object[0]);
            type =
                typeof(WorkflowDesigner).Assembly.GetType(
                    "System.Activities.Presentation.Internal.PropertyEditing.PropertyInspector");
            Inspector = (Grid)type.GetConstructor(new Type[0]).Invoke(new object[0]);
            Resources.MergedDictionaries.Add(Inspector.Resources);
            Inspector.Resources.MergedDictionaries.Add(resources);
            OnSelectionChanged = type.GetMethod("OnSelectionChanged");
            Content = Inspector;
        }

        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(
            "SelectedObject", typeof(object), typeof(WpfPropertyGrid),
            new PropertyMetadata(null, OnSelectedObjectChanged));

        private static void OnSelectedObjectChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var pg = o as WpfPropertyGrid;
            if (pg == null) return;
            // if we're a collection
            // select all objects in the collection
            // if we're an observable collection, listen for change and update our selection in that case
            // make sure that the grid updates with changes to dependency property properties on individual objects

            var objects = new List<object> {e.NewValue};
            //var objects = e.NewValue as IEnumerable ?? new[] { e.NewValue };

            if (e.OldValue is INotifyCollectionChanged)
                ((INotifyCollectionChanged)e.OldValue).CollectionChanged -= pg.PropertyGridCollectionChanged;

            if (objects is INotifyCollectionChanged)
                ((INotifyCollectionChanged)objects).CollectionChanged += pg.PropertyGridCollectionChanged;
            if (e.OldValue is IEnumerable)
                foreach (var old in ((IEnumerable)e.OldValue).OfType<INotifyPropertyChanged>())
                    (old).PropertyChanged -= pg.PropertyGridPropertyChanged;
                ;
            var mis = new List<ModelItem>();
            foreach (var obj in objects)
            {
                if (obj is INotifyPropertyChanged)
                    ((INotifyPropertyChanged)obj).PropertyChanged += pg.PropertyGridPropertyChanged;
                //if object is deleted obj is null therefore don't load and add...
                if (obj == null) continue;
                pg.ModelTreeManager.Load(obj);
                mis.Add(pg.ModelTreeManager.Root);
            }
            pg.OnSelectionChanged.Invoke(pg.Inspector, new object[] { null }); // clear previous selection
            pg.OnSelectionChanged.Invoke(pg.Inspector, new object[] { new Selection(mis) });
        }

        protected void PropertyGridPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSelectedObjectChanged(this,
                                    new DependencyPropertyChangedEventArgs(SelectedObjectProperty,
                                                                           GetValue(SelectedObjectProperty),
                                                                           GetValue(SelectedObjectProperty)));
        }

        protected void PropertyGridCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnSelectedObjectChanged(this,
                                    new DependencyPropertyChangedEventArgs(SelectedObjectProperty,
                                                                           GetValue(SelectedObjectProperty),
                                                                           GetValue(SelectedObjectProperty)));
        }

        public object SelectedObject
        {
            get { return GetValue(SelectedObjectProperty); }
            set { SetCurrentValue(SelectedObjectProperty, value); }
        }
    }
}