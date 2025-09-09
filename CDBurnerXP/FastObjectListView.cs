using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// A FastObjectListView is a virtual list that provides fast sorting and filtering.
    /// </summary>
    public class FastObjectListView : ObjectListView
    {
        /// <summary>
        /// Create a FastObjectListView
        /// </summary>
        public FastObjectListView()
        {
            this.VirtualMode = true;
            this.CacheVirtualItems += new CacheVirtualItemsEventHandler(FastObjectListView_CacheVirtualItems);
            this.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(FastObjectListView_RetrieveVirtualItem);
            this.SearchForVirtualItem += new SearchForVirtualItemEventHandler(FastObjectListView_SearchForVirtualItem);
        }

        #region Public Properties

        /// <summary>
        /// Get/set the collection of objects that this list will show
        /// </summary>
        /// <remarks>
        /// <para>
        /// The contents of the control will be updated immediately after setting this property
        /// </para>
        /// <para>
        /// Setting this property preserves selection, if possible. Use SetObjects() if
        /// you do not want to preserve the selection.
        /// </para>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IEnumerable Objects
        {
            get { return base.Objects; }
            set { this.SetObjects(value, true); }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handle the CacheVirtualItems event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FastObjectListView_CacheVirtualItems(object? sender, CacheVirtualItemsEventArgs e)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Handle the RetrieveVirtualItem event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FastObjectListView_RetrieveVirtualItem(object? sender, RetrieveVirtualItemEventArgs e)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Handle the SearchForVirtualItem event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FastObjectListView_SearchForVirtualItem(object? sender, SearchForVirtualItemEventArgs e)
        {
            // Implementation would go here
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the collection of objects that will be shown in this list view.
        /// </summary>
        /// <param name="collection">The objects to be displayed</param>
        /// <param name="preserveState">Should the state of the list be preserved?</param>
        public virtual void SetObjects(IEnumerable collection, bool preserveState)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Add the given collection of objects to the collection of objects shown in this list.
        /// </summary>
        /// <param name="modelObjects">The objects to be added</param>
        public override void AddObjects(ICollection modelObjects)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Remove the given collection of objects from the collection of objects shown in this list.
        /// </summary>
        /// <param name="modelObjects">The objects to be removed</param>
        public override void RemoveObjects(ICollection modelObjects)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Remove all items from this list
        /// </summary>
        /// <remarks>This method can safely be called from background threads.</remarks>
        public override void ClearObjects()
        {
            // Implementation would go here
        }

        /// <summary>
        /// Update the given collection of objects in the list.
        /// </summary>
        /// <param name="modelObjects">The objects to be updated</param>
        public override void UpdateObjects(ICollection modelObjects)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Update the list to reflect the contents of the given collection, preserving as much state as is possible.
        /// </summary>
        /// <param name="collection">The objects to be displayed</param>
        public virtual void UpdateVirtualListSize(IEnumerable collection)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Rebuild the list with its current contents.
        /// </summary>
        /// <remarks>
        /// Invalidate any cached information when we rebuild the list.
        /// </remarks>
        public override void BuildList(bool shouldPreserveSelection)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Sort the items by the last sort column and order
        /// </summary>
        public override void Sort()
        {
            // Implementation would go here
        }

        /// <summary>
        /// Apply the given filter to the list
        /// </summary>
        /// <param name="filter">The filter to apply</param>
        public virtual void ApplyFilter(IModelFilter filter)
        {
            // Implementation would go here
        }

        /// <summary>
        /// Remove any filter that is applied to the list
        /// </summary>
        public virtual void UnapplyFilter()
        {
            // Implementation would go here
        }

        #endregion

        #region Implementation

        /// <summary>
        /// This delegate is called to create a list item for a row object
        /// </summary>
        public delegate OLVListItem RowGetterDelegate(Object rowObject);

        #endregion
    }
}