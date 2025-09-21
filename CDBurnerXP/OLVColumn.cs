using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CDBurnerXP.Controls;

namespace CDBurnerXP.Controls
{
    /// <summary>
    /// An OLVColumn knows which aspect of an object it should present.
    /// </summary>
    /// <remarks>
    /// The column knows how to:
    /// <list>
    /// <item><description>extract its aspect from the row object</description></item>
    /// <item><description>convert its aspect to a string</description></item>
    /// <item><description>calculate the image for the row</description></item>
    /// <item><description>extract the value to be used when grouping items</description></item>
    /// <item><description>convert a group value to a display string</description></item>
    /// </list>
    /// <para>For sorting to work correctly, aspects from the same column
    /// must be of the same type, that is, the same aspect cannot sometimes
    /// return strings and other times integers.</para>
    /// </remarks>
    public partial class OLVColumn : ColumnHeader
    {
        /// <summary>
        /// Create an OLVColumn
        /// </summary>
        public OLVColumn()
            : base()
        {
        }

        /// <summary>
        /// Initialize a column to have the given title, and show the given aspect
        /// </summary>
        /// <param name="title">The title of the column</param>
        /// <param name="aspect">The aspect to be shown in the column</param>
        public OLVColumn(string title, string aspect)
            : this()
        {
            this.Text = title;
            this.AspectName = aspect;
        }

        #region Public Properties

        /// <summary>
        /// The getter of TextAlign may manipulate the value when used in the wrong moment and then switch all     
        /// left aligned column to right but *not* vice versa. This is pretty bad because there is no way to undo  
        /// the change and get the correct alignment back at a later point.
        /// Thus, save the original "intended" alignment in a separate property and restore it when the columns    
        /// are rebuilt.
        /// </summary>
        public HorizontalAlignment IntendedAlignment { get; set; }

        public new HorizontalAlignment TextAlign
        {
            get { return base.TextAlign; }
            set
            {
                base.TextAlign = value;
                this.IntendedAlignment = value;
            }
        }

        /// <summary>
        /// The name of the property or method that should be called to get the value to display in this column.   
        /// This is only used if a ValueGetterDelegate has not been given.
        /// </summary>
        /// <remarks>This name can be dotted to chain references to properties or methods.</remarks>
        /// <example>"DateOfBirth"</example>
        /// <example>"Owner.HomeAddress.Postcode"</example>
        [Category("Behavior"),
         Description("The name of the property or method that should be called to get the aspect to display in this column")]
        public string AspectName
        {
            get { return aspectName; }
            set { aspectName = value; }
        }
        private string aspectName = string.Empty;

        /// <summary>
        /// This format string will be used to convert an aspect to its string representation.
        /// </summary>
        /// <remarks>
        /// This string is passed as the first parameter to the String.Format() method.
        /// This is only used if ToStringDelegate has not been set.</remarks>
        /// <example>"{0:C}" to convert a number to currency</example>
        [Category("Behavior"),
         Description("The format string that will be used to convert an aspect to its string representation"),
         DefaultValue(null)]
        public string AspectToStringFormat
        {
            get { return aspectToStringFormat; }
            set { aspectToStringFormat = value; }
        }
        private string aspectToStringFormat = string.Empty;

        /// <summary>
        /// Group objects by the initial letter of the aspect of the column
        /// </summary>
        /// <remarks>
        /// One common pattern is to group column by the initial letter of the value for that group.
        /// The aspect must be a string (obviously).
        /// </remarks>
        [Category("Behavior"),
         Description("The name of the property or method that should be called to get the aspect to display in this column"),
         DefaultValue(false)]
        public bool UseInitialLetterForGroup
        {
            get { return useInitialLetterForGroup; }
            set { useInitialLetterForGroup = value; }
        }
        private bool useInitialLetterForGroup;

        /// <summary>
        /// Get/set whether this column should be used when the view is switched to tile view.
        /// </summary>
        /// <remarks>Column 0 is always included in tileview regardless of this setting.
        /// Tile views do not work well with many "columns" of information, 2 or 3 works best.</remarks>
        [Category("Behavior"),
        Description("Will this column be used when the view is switched to tile view"),
         DefaultValue(false)]
        public bool IsTileViewColumn
        {
            get { return isTileViewColumn; }
            set { isTileViewColumn = value; }
        }
        private bool isTileViewColumn = false;

        /// <summary>
        /// This delegate will be used to extract a value to be displayed in this column.
        /// </summary>
        /// <remarks>
        /// If this is set, AspectName is ignored.
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectGetterDelegate? AspectGetter
        {
            get { return aspectGetter; }
            set
            {
                aspectGetter = value;
                aspectGetterAutoGenerated = false;
            }
        }
        private AspectGetterDelegate? aspectGetter;

        /// <summary>
        /// The delegate that will be used to translate the aspect to display in this column into a string.        
        /// </summary>
        /// <remarks>If this value is set, ValueToStringFormat will be ignored.</remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectToStringConverterDelegate? AspectToStringConverter
        {
            get { return aspectToStringConverter; }
            set { aspectToStringConverter = value; }
        }
        private AspectToStringConverterDelegate? aspectToStringConverter;

        /// <summary>
        /// This delegate is called to get the image selector of the image that should be shown in this column.    
        /// It can return an int, string, Image or null.
        /// </summary>
        /// <remarks><para>This delegate can use these return value to identify the image:</para>
        /// <list>
        /// <item>null or -1 -- indicates no image</item>
        /// <item>an int -- the int value will be used as an index into the image list</item>
        /// <item>a String -- the string value will be used as a key into the image list</item>
        /// <item>an Image -- the Image will be drawn directly (only in OwnerDrawn mode)</item>
        /// </list>
        /// </remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImageGetterDelegate? ImageGetter
        {
            get { return imageGetter; }
            set { imageGetter = value; }
        }
        private ImageGetterDelegate? imageGetter;

        /// <summary>
        /// This delegate is called to get the object that is the key for the group
        /// to which the given row belongs.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GroupKeyGetterDelegate? GroupKeyGetter
        {
            get { return groupKeyGetter; }
            set { groupKeyGetter = value; }
        }
        private GroupKeyGetterDelegate? groupKeyGetter;

        /// <summary>
        /// This delegate is called to convert a group key into a display string.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GroupKeyToTitleConverterDelegate? GroupKeyToTitleConverter
        {
            get { return groupKeyToTitleConverter; }
            set { groupKeyToTitleConverter = value; }
        }
        private GroupKeyToTitleConverterDelegate? groupKeyToTitleConverter;

        /// <summary>
        /// This delegate is called to put a modified aspect back into the model object.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AspectPutterDelegate? AspectPutter
        {
            get { return aspectPutter; }
            set { aspectPutter = value; }
        }
        private AspectPutterDelegate? aspectPutter;

        /// <summary>
        /// This delegate is called to get the editor that should be used to edit the value of this column.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EditorCreatorDelegate? EditorCreator
        {
            get { return editorCreator; }
            set { editorCreator = value; }
        }
        private EditorCreatorDelegate? editorCreator;

        /// <summary>
        /// This delegate is called to get the renderer that should be used to draw this column.
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RendererDelegate? ColumnRendererDelegate
        {
            get { return rendererDelegate; }
            set { rendererDelegate = value; }
        }
        private RendererDelegate? rendererDelegate;

        /// <summary>
        /// This renderer will be used to draw this column.
        /// </summary>
        /// <remarks>If this is set, RendererDelegate is ignored.</remarks>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IRenderer? Renderer
        {
            get { return renderer; }
            set { renderer = value; }
        }
        private IRenderer? renderer;

        /// <summary>
        /// The format to use when suffixing the count of items in a group with the
        /// label "item" or "items".
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? GroupWithItemCountFormat
        {
            get { return groupWithItemCountFormat; }
            set { groupWithItemCountFormat = value; }
        }
        private string? groupWithItemCountFormat;

        /// <summary>
        /// The format to use when suffixing the count of items in a group with the
        /// label "item" (when there is only a single item in the group).
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? GroupWithItemCountSingularFormat
        {
            get { return groupWithItemCountSingularFormat; }
            set { groupWithItemCountSingularFormat = value; }
        }
        private string? groupWithItemCountSingularFormat;

        /// <summary>
        /// Some declarations for delegates used within OLVColumn
        /// </summary>
        public delegate Object AspectGetterDelegate(Object rowObject);
        public delegate string AspectToStringConverterDelegate(Object value);
        public delegate Object ImageGetterDelegate(Object rowObject);
        public delegate Object GroupKeyGetterDelegate(Object rowObject);
        public delegate string GroupKeyToTitleConverterDelegate(Object value);
        public delegate void AspectPutterDelegate(Object rowObject, Object newValue);
        public delegate Control? EditorCreatorDelegate(Object rowObject);
        public delegate bool RendererDelegate(DrawListViewSubItemEventArgs e, Graphics g, Rectangle r, Object rowObject);

        /// <summary>
        /// Was the AspectGetter for this column automatically generated?
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AspectGetterAutoGenerated
        {
            get { return aspectGetterAutoGenerated; }
            set { aspectGetterAutoGenerated = value; }
        }
        private bool aspectGetterAutoGenerated = true;

        /*
        /// <summary>
        /// Gets or sets the format to use when suffixing item counts to group titles
        /// </summary>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountFormat
        {
            get { return groupWithItemCountFormat; }
            set { groupWithItemCountFormat = value; }
        }
        private string groupWithItemCountFormat;

        /// <summary>
        /// Return this.GroupWithItemCountFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountFormat))
                    return "{0} [{1} items]";
                else
                    return this.GroupWithItemCountFormat;
            }
        }

        /// <summary>
        /// Gets or sets the format to use when suffixing item counts to group titles
        /// when there is only one item in the group
        /// </summary>
        [Category("Behavior"),
         Description("The format to use when suffixing item counts to group titles"),
         DefaultValue(null)]
        public string GroupWithItemCountSingularFormat
        {
            get { return groupWithItemCountSingularFormat; }
            set { groupWithItemCountSingularFormat = value; }
        }
        private string groupWithItemCountSingularFormat;

        /// <summary>
        /// Return this.GroupWithItemCountSingularFormat or a reasonable default
        /// </summary>
        [Browsable(false)]
        public string GroupWithItemCountSingularFormatOrDefault
        {
            get
            {
                if (String.IsNullOrEmpty(this.GroupWithItemCountSingularFormat))
                    return "{0} [{1} item]";
                else
                    return this.GroupWithItemCountSingularFormat;
            }
        }
        */

        /// <summary>
        /// Gets or sets the minimum width that this column can be
        /// </summary>
        [Category("Behavior"),
         Description("The minimum width that this column can be"),
         DefaultValue(0)]
        public int MinimumWidth
        {
            get { return minimumWidth; }
            set { minimumWidth = value; }
        }
        private int minimumWidth = 0;

        /// <summary>
        /// Gets or sets the maximum width that this column can be
        /// </summary>
        [Category("Behavior"),
         Description("The maximum width that this column can be"),
         DefaultValue(0)]
        public int MaximumWidth
        {
            get { return maximumWidth; }
            set { maximumWidth = value; }
        }
        private int maximumWidth = 0;

        /// <summary>
        /// Gets or sets whether this column has a fixed width
        /// </summary>
        [Category("Behavior"),
         Description("Is the width of this column fixed?"),
         DefaultValue(false)]
        public bool IsFixedWidth
        {
            get { return isFixedWidth; }
            set { isFixedWidth = value; }
        }
        private bool isFixedWidth = false;

        /// <summary>
        /// What proportion of the free space should this column occupy?
        /// </summary>
        [Category("Behavior"),
         Description("What proportion of the free space should this column occupy?"),
         DefaultValue(0)]
        public int FreeSpaceProportion
        {
            get { return freeSpaceProportion; }
            set { freeSpaceProportion = value; }
        }
        private int freeSpaceProportion = 0;

        /// <summary>
        /// Gets or sets whether this column will be automatically resized to fill the free space
        /// of the listview
        /// </summary>
        [Category("Behavior"),
         Description("Will this column be automatically resized to fill the free space of the listview?"),
         DefaultValue(false)]
        public bool FillsFreeSpace
        {
            get { return fillsFreeSpace; }
            set { fillsFreeSpace = value; }
        }
        private bool fillsFreeSpace = false;

        /// <summary>
        /// Gets or sets whether this column is editable
        /// </summary>
        [Category("Behavior"),
         Description("Can the values shown in this column be edited?"),
         DefaultValue(true)]
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }
        private bool isEditable = true;

        /// <summary>
        /// Gets or sets the control that should be used to edit the value in this column
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control CellEditor
        {
            get { return cellEditor; }
            set { cellEditor = value; }
        }
        private Control cellEditor = new Control();

        /// <summary>
        /// Gets or sets whether this column should be visible
        /// </summary>
        [Category("Behavior"),
         Description("Should this column be visible?"),
         DefaultValue(true)]
        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        private bool isVisible = true;

        /// <summary>
        /// When a column is removed from the control, it remembers its last display index
        /// so it can be restored to the same position later
        /// </summary>
        public int LastDisplayIndex = -1;

        #endregion

        #region Public Methods

        /// <summary>
        /// For a given row object, extract the value indicated by the AspectName property of this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>The value that should be displayed in this column</returns>
        public object? GetValue(object rowObject)
        {
            if (this.AspectGetter != null)
                return this.AspectGetter(rowObject);
            else if (!String.IsNullOrEmpty(this.AspectName))
                return this.GetAspectByName(rowObject);
            else
                return null;
        }

        /// <summary>
        /// Extract the value indicated by the AspectName property of this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>The value that should be displayed in this column</returns>
        public object? GetAspectByName(object rowObject)
        {
            if (rowObject == null)
                return null;

            // Split the AspectName and handle each part
            string[] parts = this.AspectName.Split('.');
            object currentObject = rowObject;

            foreach (string part in parts)
            {
                if (currentObject == null)
                    return null;

                Type type = currentObject.GetType();
                System.Reflection.PropertyInfo? propertyInfo = type.GetProperty(part);
                if (propertyInfo != null)
                {
                    currentObject = propertyInfo.GetValue(currentObject, null) ?? currentObject;
                }
                else
                {
                    System.Reflection.MethodInfo methodInfo = type.GetMethod(part);
                    if (methodInfo != null)
                    {
                        object? methodResult = methodInfo.Invoke(currentObject, null);
                        if (methodResult != null)
                            currentObject = methodResult;
                        else
                            return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return currentObject;
        }

        /// <summary>
        /// Put the given value into the aspect of the given row object
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <param name="newValue">The new value for the aspect</param>
        public void PutValue(Object rowObject, Object newValue)
        {
            if (this.AspectPutter != null)
                this.AspectPutter(rowObject, newValue);
            else if (!String.IsNullOrEmpty(this.AspectName))
                this.PutAspectByName(rowObject, newValue);
        }

        /// <summary>
        /// Put the given value into the aspect of the given row object
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <param name="newValue">The new value for the aspect</param>
        public void PutAspectByName(Object rowObject, Object newValue)
        {
            if (rowObject == null || String.IsNullOrEmpty(this.AspectName))
                return;

            // Split the AspectName and handle each part
            string[] parts = this.AspectName.Split('.');
            object currentObject = rowObject;

            // Navigate to the parent object of the final property
            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (currentObject == null)
                    return;

                Type type = currentObject.GetType();
                System.Reflection.PropertyInfo? propertyInfo = type.GetProperty(parts[i]);
                if (propertyInfo != null)
                {
                    currentObject = propertyInfo.GetValue(currentObject, null) ?? currentObject;
                }
                else
                {
                    return;
                }
            }

            // Set the final property
            if (currentObject != null)
            {
                Type type = currentObject.GetType();
                System.Reflection.PropertyInfo? propertyInfo = type.GetProperty(parts[parts.Length - 1]);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(currentObject, newValue, null);
                }
            }
        }

        /// <summary>
        /// Convert the aspect value to its string representation
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>A string representation of the aspect value</returns>
        public string GetStringValue(object rowObject)
        {
            object? aspect = this.GetValue(rowObject);
            return aspect != null ? this.ValueToString(aspect) : string.Empty;
        }

        /// <summary>
        /// Convert the given value to its string representation
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <returns>A string representation of the value</returns>
        public string ValueToString(object value)
        {
            if (value == null)
                return "";

            if (this.AspectToStringConverter != null)
                return this.AspectToStringConverter(value) ?? value.ToString() ?? string.Empty;
            else if (!String.IsNullOrEmpty(this.AspectToStringFormat))
                return String.Format(this.AspectToStringFormat, value);
            else
                return value.ToString();
        }

        /// <summary>
        /// For a given row object, return the image selector of the image that should be shown in this column.
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>The image selector for the image that should be shown</returns>
        public Object? GetImage(object rowObject)
        {
            if (this.ImageGetter != null)
                return this.ImageGetter(rowObject);
            else
                return null;
        }

        /// <summary>
        /// For a given row object, return the object that is the key for the group to which the row belongs
        /// </summary>
        /// <param name="rowObject">The row object that is being displayed</param>
        /// <returns>The group key for the row</returns>
        public object? GetGroupKey(object rowObject)
        {
            if (this.GroupKeyGetter != null)
                return this.GroupKeyGetter(rowObject);
            else if (!String.IsNullOrEmpty(this.AspectName))
                return this.GetValue(rowObject) ?? null;
            else
                return null;
        }

        /// <summary>
        /// Convert a group key into a title for that group
        /// </summary>
        /// <param name="value">The group key that is being converted</param>
        /// <returns>The title for the group</returns>
        public string ConvertGroupKeyToTitle(object value)
        {
            if (this.GroupKeyToTitleConverter != null)
                return this.GroupKeyToTitleConverter(value) ?? value.ToString() ?? string.Empty;
            else if (value == null)
                return "{null}";
            else
                return value.ToString();
        }

        /// <summary>
        /// Create groupies for this column
        /// </summary>
        /// <typeparam name="T">The type of values that are being grouped</typeparam>
        /// <param name="values">The values that are to be grouped</param>
        /// <param name="descriptions">The descriptions for each group</param>
        public void MakeGroupies<T>(T[] values, string[] descriptions)
        {
            // Implementation would go here
        }

        #endregion
    }
}