// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2005 Novell, Inc. (http://www.novell.com)
//
// Author:
//	Pedro Martínez Juliá <pedromj@gmail.com>
//


#if NET_2_0

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms {

	public class DataGridViewCheckBoxCell : DataGridViewCell, IDataGridViewEditingCell {

		private object editingCellFormattedValue;
		private bool editingCellValueChanged;
		private object falseValue;
		private FlatStyle flatStyle;
		private object indeterminateValue;
		private bool threeState;
		private object trueValue;
		private Type valueType;
		private PushButtonState check_state;

		public DataGridViewCheckBoxCell ()
		{
			check_state = PushButtonState.Normal;
			editingCellValueChanged = false;
			falseValue = null;
			flatStyle = FlatStyle.Standard;
			indeterminateValue = null;
			threeState = false;
			trueValue = null;
			valueType = null;
		}

		public DataGridViewCheckBoxCell (bool threeState) : this()
		{
			this.threeState = threeState;
		}

		public virtual object EditingCellFormattedValue {
			get { return editingCellFormattedValue; }
			set {
				if (FormattedValueType == null || value == null || value.GetType() != FormattedValueType || !(value is Boolean) || !(value is CheckState)) {
					throw new ArgumentException("Cannot set this property.");
				}
				editingCellFormattedValue = value;
			}
		}

		public virtual bool EditingCellValueChanged {
			get { return editingCellValueChanged; }
			set { editingCellValueChanged = value; }
		}

		public override Type EditType {
			get { return null; }
		}

		[DefaultValue (null)]
		public object FalseValue {
			get { return falseValue; }
			set { falseValue = value; }
		}

		[DefaultValue (FlatStyle.Standard)]
		public FlatStyle FlatStyle {
			get { return flatStyle; }
			set {
				if (!Enum.IsDefined(typeof(FlatStyle), value)) {
					throw new InvalidEnumArgumentException("Value is not valid FlatStyle.");
				}
				if (value == FlatStyle.Popup) {
					throw new Exception("FlatStyle cannot be set to Popup in this control.");
				}
			}
		}

		public override Type FormattedValueType {
			get {
				if (ThreeState) {
					return typeof(CheckState);
				}
				return typeof(Boolean);
			}
		}

		[DefaultValue (null)]
		public object IndeterminateValue {
			get { return indeterminateValue; }
			set { indeterminateValue = value; }
		}

		[DefaultValue (false)]
		public bool ThreeState {
			get { return threeState; }
			set { threeState = value; }
		}

		[DefaultValue (null)]
		public object TrueValue {
			get { return trueValue; }
			set { trueValue = value; }
		}

		public override Type ValueType {
			get {
				if (valueType == null) {
					if (OwningColumn != null && OwningColumn.ValueType != null) {
						return OwningColumn.ValueType;
					}
					if (ThreeState) {
						return typeof(CheckState);
					}
					return typeof(Boolean);
				}
				return valueType;
			}
			set { valueType = value; }
		}

		public override object Clone ()
		{
			DataGridViewCheckBoxCell cell = (DataGridViewCheckBoxCell) base.Clone();
			cell.editingCellValueChanged = this.editingCellValueChanged;
			cell.falseValue = this.falseValue;
			cell.flatStyle = this.flatStyle;
			cell.indeterminateValue = this.indeterminateValue;
			cell.threeState = this.threeState;
			cell.trueValue = this.trueValue;
			cell.valueType = this.valueType;
			return cell;
		}

		public virtual object GetEditingCellFormattedValue (DataGridViewDataErrorContexts context)
		{
			if (FormattedValueType == null) {
				throw new InvalidOperationException("FormattedValueType is null.");
			}
			if ((context & DataGridViewDataErrorContexts.ClipboardContent) != 0) {
				return Convert.ToString(Value);
			}
			if (ThreeState) {
				return (CheckState) Value;
			}
			return (Boolean) Value;
		}

		public override object ParseFormattedValue (object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
		{
			if (cellStyle == null) {
				throw new ArgumentNullException("CellStyle is null");
			}
			if (FormattedValueType == null) {
				throw new FormatException("FormattedValueType is null.");
			}
			if (formattedValue == null || formattedValue.GetType() != FormattedValueType) {
				throw new ArgumentException("FormattedValue is null or is not instance of FormattedValueType.");
			}
			
			return base.ParseFormattedValue (formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
		}

		public virtual void PrepareEditingCellForEdit (bool selectAll)
		{
		}

		public override string ToString ()
		{
			return string.Format ("DataGridViewCheckBoxCell {{ ColumnIndex={0}, RowIndex={1} }}", ColumnIndex, RowIndex);
		}

		protected override bool ContentClickUnsharesRow (DataGridViewCellEventArgs e)
		{
			return this.IsInEditMode;
		}

		protected override bool ContentDoubleClickUnsharesRow (DataGridViewCellEventArgs e)
		{
			return this.IsInEditMode;
		}

		protected override AccessibleObject CreateAccessibilityInstance ()
		{
			return new DataGridViewCheckBoxCellAccessibleObject(this);
		}

		protected override Rectangle GetContentBounds (Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (DataGridView == null)
				return Rectangle.Empty;

			return new Rectangle ((Size.Width - 13) / 2, (Size.Height - 13) / 2, 13, 13);
		}

		protected override Rectangle GetErrorIconBounds (Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
		{
			if (DataGridView == null || string.IsNullOrEmpty (ErrorText))
				return Rectangle.Empty;

			Size error_icon = new Size (12, 11);
			return new Rectangle (new Point (Size.Width - error_icon.Width - 5, (Size.Height - error_icon.Height) / 2), error_icon);
		}

		protected override object GetFormattedValue (object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
		{
			if (DataGridView == null)
				return null;
			
			if (value == falseValue)
				return false;
			if (value == trueValue)
				return true;
				
			return Convert.ToBoolean (value);
		}

		protected override Size GetPreferredSize (Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
		{
			return new Size (21, 20);
		}

		protected override bool KeyDownUnsharesRow (KeyEventArgs e, int rowIndex)
		{
			// true if the user pressed the SPACE key without modifier keys; otherwise, false
			return e.KeyData == Keys.Space;
		}

		protected override bool KeyUpUnsharesRow (KeyEventArgs e, int rowIndex)
		{
			// true if the user released the SPACE key; otherwise false
			return e.KeyData == Keys.Space;
		}

		protected override bool MouseDownUnsharesRow (DataGridViewCellMouseEventArgs e)
		{
			return (e.Button == MouseButtons.Left);
		}

		protected override bool MouseEnterUnsharesRow (int rowIndex)
		{
			// true if the cell was the last cell receiving a mouse click; otherwise, false.
			return false;
		}

		protected override bool MouseLeaveUnsharesRow (int rowIndex)
		{
			// true if the button displayed by the cell is in the pressed state; otherwise, false.
			return check_state == PushButtonState.Pressed;
		}

		protected override bool MouseUpUnsharesRow (DataGridViewCellMouseEventArgs e)
		{
			// true if the mouse up was caused by the release of the left mouse button; otherwise false.
			return e.Button == MouseButtons.Left;
		}

		protected override void OnContentClick (DataGridViewCellEventArgs e)
		{
			DataGridViewCellStyle current_style = InheritedStyle;
			
			bool current = (bool)GetFormattedValue (Value, e.RowIndex, ref current_style, null, null, DataGridViewDataErrorContexts.Parsing);
			
			if (current)
				Value = falseValue == null ? false : falseValue;
			else
				Value = trueValue == null ? true : trueValue;
		}

		protected override void OnContentDoubleClick (DataGridViewCellEventArgs e)
		{
		}

		protected override void OnKeyDown (KeyEventArgs e, int rowIndex)
		{
			// when activated by the SPACE key, this method updates the cell's user interface
			if ((e.KeyData & Keys.Space) == Keys.Space) {
				check_state = PushButtonState.Pressed;
				DataGridView.InvalidateCell (this);
			}
		}

		protected override void OnKeyUp (KeyEventArgs e, int rowIndex)
		{
			// when activated by the SPACE key, this method updates the cell's user interface
			if ((e.KeyData & Keys.Space) == Keys.Space) {
				check_state = PushButtonState.Normal;
				DataGridView.InvalidateCell (this);
			}
		}

		protected override void OnLeave (int rowIndex, bool throughMouseClick)
		{
			if (check_state != PushButtonState.Normal) {
				check_state = PushButtonState.Normal;
				DataGridView.InvalidateCell (this);
			}
		}

		protected override void OnMouseDown (DataGridViewCellMouseEventArgs e)
		{
			// if activated by depresing the left mouse button, this method updates the cell's user interface
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				check_state = PushButtonState.Pressed;
				DataGridView.InvalidateCell (this);
			}
		}

		protected override void OnMouseLeave (int rowIndex)
		{
			// if the cell's button is not in its normal state, this method causes the cell's user interface to be updated.
			if (check_state != PushButtonState.Normal) {
				check_state = PushButtonState.Normal;
				DataGridView.InvalidateCell (this);
			}
		}

		protected override void OnMouseMove (DataGridViewCellMouseEventArgs e)
		{
			if (check_state != PushButtonState.Normal && check_state != PushButtonState.Hot) {
				check_state = PushButtonState.Hot;
				DataGridView.InvalidateCell (this);
			}
		}

		protected override void OnMouseUp (DataGridViewCellMouseEventArgs e)
		{
			// if activated by the left mouse button, this method updates the cell's user interface
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				check_state = PushButtonState.Normal;
				DataGridView.InvalidateCell (this);
			}
		}

		protected override void Paint (Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
			base.Paint (graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
		}

		internal override void PaintPartContent (Graphics graphics, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, DataGridViewCellStyle cellStyle, object formattedValue)
		{
			CheckBoxState state;
			
			if ((bool)formattedValue == false)
				state = (CheckBoxState)check_state;
			else if ((bool)formattedValue == true)
				state = (CheckBoxState)((int)check_state + 4);
			else
				state = (CheckBoxState)((int)check_state + 8);
			
			Point p = new Point (cellBounds.X + (Size.Width - 13) / 2, cellBounds.Y + (Size.Height - 13) / 2);
			CheckBoxRenderer.DrawCheckBox (graphics, p, state);
		}
		
		protected class DataGridViewCheckBoxCellAccessibleObject : DataGridViewCellAccessibleObject {

			public DataGridViewCheckBoxCellAccessibleObject (DataGridViewCell owner) : base(owner)
			{
			}

			public override string DefaultAction {
				get {
					if (Owner.ReadOnly) {
						return "";
					}
					// return "Press to check" if the check box is not selected
					// and "Press to uncheck" if the check box is selected
					throw new NotImplementedException();
				}
			}

			public override void DoDefaultAction ()
			{
				// change the state of the check box
			}

			public override int GetChildCount ()
			{
				return -1;
			}

		}

	}

}

#endif
