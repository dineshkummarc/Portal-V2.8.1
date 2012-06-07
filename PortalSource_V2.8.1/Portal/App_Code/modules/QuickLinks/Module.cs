﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Portal.Modules.QuickLinks {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class Module : DataSet {
        
        private LinksDataTable tableLinks;
        
        public Module() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected Module(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["Links"] != null)) {
                    this.Tables.Add(new LinksDataTable(ds.Tables["Links"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.InitClass();
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public LinksDataTable Links {
            get {
                return this.tableLinks;
            }
        }
        
        public override DataSet Clone() {
            Module cln = ((Module)(base.Clone()));
            cln.InitVars();
            return cln;
        }
        
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        protected override void ReadXmlSerializable(XmlReader reader) {
            this.Reset();
            DataSet ds = new DataSet();
            ds.ReadXml(reader);
            if ((ds.Tables["Links"] != null)) {
                this.Tables.Add(new LinksDataTable(ds.Tables["Links"]));
            }
            this.DataSetName = ds.DataSetName;
            this.Prefix = ds.Prefix;
            this.Namespace = ds.Namespace;
            this.Locale = ds.Locale;
            this.CaseSensitive = ds.CaseSensitive;
            this.EnforceConstraints = ds.EnforceConstraints;
            this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
            this.InitVars();
        }
        
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new XmlTextReader(stream), null);
        }
        
        public void InitVars() {
            this.tableLinks = ((LinksDataTable)(this.Tables["Links"]));
            if ((this.tableLinks != null)) {
                this.tableLinks.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "Module";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/Module.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableLinks = new LinksDataTable();
            this.Tables.Add(this.tableLinks);
        }
        
        private bool ShouldSerializelinks() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void linksRowChangeEventHandler(object sender, linksRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class LinksDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnPosition;
            
            private DataColumn columnText;
            
            private DataColumn columnURL;
            
            private DataColumn columnOpenInNewWindow;
            
            public LinksDataTable() : 
                    base("Links") {
                this.InitClass();
            }
            
            public LinksDataTable(DataTable table) : 
                    base(table.TableName) {
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
                this.DisplayExpression = table.DisplayExpression;
            }
            
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            public DataColumn PositionColumn {
                get {
                    return this.columnPosition;
                }
            }
            
            public DataColumn TextColumn {
                get {
                    return this.columnText;
                }
            }
            
            public DataColumn URLColumn {
                get {
                    return this.columnURL;
                }
            }
            
            public DataColumn OpenInNewWindowColumn {
                get {
                    return this.columnOpenInNewWindow;
                }
            }
            
            public linksRow this[int index] {
                get {
                    return ((linksRow)(this.Rows[index]));
                }
            }
            
            public event linksRowChangeEventHandler linksRowChanged;
            
            public event linksRowChangeEventHandler linksRowChanging;
            
            public event linksRowChangeEventHandler linksRowDeleted;
            
            public event linksRowChangeEventHandler linksRowDeleting;
            
            public void AddlinksRow(linksRow row) {
                this.Rows.Add(row);
            }
            
            public linksRow AddlinksRow(int Position, string Text, string URL, bool OpenInNewWindow) {
                linksRow rowlinksRow = ((linksRow)(this.NewRow()));
                rowlinksRow.ItemArray = new object[] {
                        Position,
                        Text,
                        URL,
                        OpenInNewWindow};
                this.Rows.Add(rowlinksRow);
                return rowlinksRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                LinksDataTable cln = ((LinksDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new LinksDataTable();
            }
            
            public void InitVars() {
                this.columnPosition = this.Columns["Position"];
                this.columnText = this.Columns["Text"];
                this.columnURL = this.Columns["URL"];
                this.columnOpenInNewWindow = this.Columns["OpenInNewWindow"];
            }
            
            private void InitClass() {
                this.columnPosition = new DataColumn("Position", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnPosition);
                this.columnText = new DataColumn("Text", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnText);
                this.columnURL = new DataColumn("URL", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnURL);
                this.columnOpenInNewWindow = new DataColumn("OpenInNewWindow", typeof(bool), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnOpenInNewWindow);
            }
            
            public linksRow NewlinksRow() {
                return ((linksRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new linksRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(linksRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.linksRowChanged != null)) {
                    this.linksRowChanged(this, new linksRowChangeEvent(((linksRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.linksRowChanging != null)) {
                    this.linksRowChanging(this, new linksRowChangeEvent(((linksRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.linksRowDeleted != null)) {
                    this.linksRowDeleted(this, new linksRowChangeEvent(((linksRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.linksRowDeleting != null)) {
                    this.linksRowDeleting(this, new linksRowChangeEvent(((linksRow)(e.Row)), e.Action));
                }
            }
            
            public void RemovelinksRow(linksRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class linksRow : DataRow {
            
            private LinksDataTable tableLinks;
            
            public linksRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableLinks = ((LinksDataTable)(this.Table));
            }
            
            public int Position {
                get {
                    try {
                        return ((int)(this[this.tableLinks.PositionColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableLinks.PositionColumn] = value;
                }
            }
            
            public string Text {
                get {
                    try {
                        return ((string)(this[this.tableLinks.TextColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableLinks.TextColumn] = value;
                }
            }
            
            public string URL {
                get {
                    try {
                        return ((string)(this[this.tableLinks.URLColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableLinks.URLColumn] = value;
                }
            }
            
            public bool OpenInNewWindow {
                get {
                    try {
                        return ((bool)(this[this.tableLinks.OpenInNewWindowColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableLinks.OpenInNewWindowColumn] = value;
                }
            }
            
            public bool IsPositionNull() {
                return this.IsNull(this.tableLinks.PositionColumn);
            }
            
            public void SetPositionNull() {
                this[this.tableLinks.PositionColumn] = System.Convert.DBNull;
            }
            
            public bool IsTextNull() {
                return this.IsNull(this.tableLinks.TextColumn);
            }
            
            public void SetTextNull() {
                this[this.tableLinks.TextColumn] = System.Convert.DBNull;
            }
            
            public bool IsURLNull() {
                return this.IsNull(this.tableLinks.URLColumn);
            }
            
            public void SetURLNull() {
                this[this.tableLinks.URLColumn] = System.Convert.DBNull;
            }
            
            public bool IsOpenInNewWindowNull() {
                return this.IsNull(this.tableLinks.OpenInNewWindowColumn);
            }
            
            public void SetOpenInNewWindowNull() {
                this[this.tableLinks.OpenInNewWindowColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class linksRowChangeEvent : EventArgs {
            
            private linksRow eventRow;
            
            private DataRowAction eventAction;
            
            public linksRowChangeEvent(linksRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public linksRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            public DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
