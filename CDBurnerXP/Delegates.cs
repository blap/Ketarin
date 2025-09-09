using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// Delegates for the ObjectListView and related classes
    /// </summary>
    
    /// <summary>
    /// These delegates are used to extract values to be displayed in the list.
    /// </summary>
    public delegate Object AspectGetterDelegate(Object rowObject);
    
    /// <summary>
    /// These delegates are used to convert an aspect value to a string.
    /// </summary>
    public delegate string AspectToStringConverterDelegate(Object value);
    
    /// <summary>
    /// These delegates are used to get the image selector for a row.
    /// </summary>
    public delegate Object ImageGetterDelegate(Object rowObject);
    
    /// <summary>
    /// These delegates are used to get the group key for a row.
    /// </summary>
    public delegate Object GroupKeyGetterDelegate(Object rowObject);
    
    /// <summary>
    /// These delegates are used to convert a group key to a title.
    /// </summary>
    public delegate string GroupKeyToTitleConverterDelegate(Object value);
    
    /// <summary>
    /// These delegates are used to render a cell in OwnerDrawn mode.
    /// </summary>
    public delegate bool RenderDelegate(DrawListViewSubItemEventArgs e, Graphics g, Rectangle r, Object rowObject);
    
    /// <summary>
    /// These delegates are used to put an edited value back into the model object.
    /// </summary>
    public delegate void AspectPutterDelegate(Object rowObject, Object newValue);
    
    /// <summary>
    /// These delegates are used to create a list item for a row object in FastObjectListView.
    /// </summary>
    public delegate OLVListItem RowGetterDelegate(Object rowObject);
    
    /// <summary>
    /// Interface for model filters
    /// </summary>
    public interface IModelFilter
    {
        /// <summary>
        /// Should the given model be included when this filter is installed
        /// </summary>
        /// <param name="modelObject">The model object to consider</param>
        /// <returns>Returns true if the model should be included</returns>
        bool Filter(object modelObject);
    }
    
    /// <summary>
    /// Interface for list filters
    /// </summary>
    public interface IListFilter
    {
        /// <summary>
        /// Return the indices of the rows that should be included when this filter is installed
        /// </summary>
        /// <param name="list">The list to be filtered</param>
        /// <returns>A list of indices of rows that should be included</returns>
        List<int> Filter(ObjectListView list);
    }
}