﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.2032
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace Portal.Modules.Forum.Data {
    using System;
    using System.Data;
    using System.Xml;
    using System.Runtime.Serialization;
    
    
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.ComponentModel.ToolboxItem(true)]
    public class UserProfileData : DataSet {
        
        private UserDataDataTable tableUserData;
        
        public UserProfileData() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected UserProfileData(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["UserData"] != null)) {
                    this.Tables.Add(new UserDataDataTable(ds.Tables["UserData"]));
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
        public UserDataDataTable UserData {
            get {
                return this.tableUserData;
            }
        }
        
        public override DataSet Clone() {
            UserProfileData cln = ((UserProfileData)(base.Clone()));
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
            if ((ds.Tables["UserData"] != null)) {
                this.Tables.Add(new UserDataDataTable(ds.Tables["UserData"]));
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
            this.tableUserData = ((UserDataDataTable)(this.Tables["UserData"]));
            if ((this.tableUserData != null)) {
                this.tableUserData.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "UserProfile";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/UserProfileData.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableUserData = new UserDataDataTable();
            this.Tables.Add(this.tableUserData);
        }
        
        private bool ShouldSerializeUserData() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void UserDataRowChangeEventHandler(object sender, UserDataRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class UserDataDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnUserID;
            
            private DataColumn columnDisplayName;
            
            private DataColumn columnEmail;
            
            private DataColumn columnImageFileName;
            
            public UserDataDataTable() : 
                    base("UserData") {
                this.InitClass();
            }
            
            public UserDataDataTable(DataTable table) : 
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
            
            public DataColumn UserIDColumn {
                get {
                    return this.columnUserID;
                }
            }
            
            public DataColumn DisplayNameColumn {
                get {
                    return this.columnDisplayName;
                }
            }
            
            public DataColumn EmailColumn {
                get {
                    return this.columnEmail;
                }
            }
            
            public DataColumn ImageFileNameColumn {
                get {
                    return this.columnImageFileName;
                }
            }
            
            public UserDataRow this[int index] {
                get {
                    return ((UserDataRow)(this.Rows[index]));
                }
            }
            
            public event UserDataRowChangeEventHandler UserDataRowChanged;
            
            public event UserDataRowChangeEventHandler UserDataRowChanging;
            
            public event UserDataRowChangeEventHandler UserDataRowDeleted;
            
            public event UserDataRowChangeEventHandler UserDataRowDeleting;
            
            public void AddUserDataRow(UserDataRow row) {
                this.Rows.Add(row);
            }
            
            public UserDataRow AddUserDataRow(string UserID, string DisplayName, string Email, string ImageFileName) {
                UserDataRow rowUserDataRow = ((UserDataRow)(this.NewRow()));
                rowUserDataRow.ItemArray = new object[] {
                        UserID,
                        DisplayName,
                        Email,
                        ImageFileName};
                this.Rows.Add(rowUserDataRow);
                return rowUserDataRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                UserDataDataTable cln = ((UserDataDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new UserDataDataTable();
            }
            
            public void InitVars() {
                this.columnUserID = this.Columns["UserID"];
                this.columnDisplayName = this.Columns["DisplayName"];
                this.columnEmail = this.Columns["Email"];
                this.columnImageFileName = this.Columns["ImageFileName"];
            }
            
            private void InitClass() {
                this.columnUserID = new DataColumn("UserID", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnUserID);
                this.columnDisplayName = new DataColumn("DisplayName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDisplayName);
                this.columnEmail = new DataColumn("Email", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnEmail);
                this.columnImageFileName = new DataColumn("ImageFileName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnImageFileName);
            }
            
            public UserDataRow NewUserDataRow() {
                return ((UserDataRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new UserDataRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(UserDataRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.UserDataRowChanged != null)) {
                    this.UserDataRowChanged(this, new UserDataRowChangeEvent(((UserDataRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.UserDataRowChanging != null)) {
                    this.UserDataRowChanging(this, new UserDataRowChangeEvent(((UserDataRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.UserDataRowDeleted != null)) {
                    this.UserDataRowDeleted(this, new UserDataRowChangeEvent(((UserDataRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.UserDataRowDeleting != null)) {
                    this.UserDataRowDeleting(this, new UserDataRowChangeEvent(((UserDataRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveUserDataRow(UserDataRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class UserDataRow : DataRow {
            
            private UserDataDataTable tableUserData;
            
            public UserDataRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableUserData = ((UserDataDataTable)(this.Table));
            }
            
            public string UserID {
                get {
                    try {
                        return ((string)(this[this.tableUserData.UserIDColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableUserData.UserIDColumn] = value;
                }
            }
            
            public string DisplayName {
                get {
                    try {
                        return ((string)(this[this.tableUserData.DisplayNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableUserData.DisplayNameColumn] = value;
                }
            }
            
            public string Email {
                get {
                    try {
                        return ((string)(this[this.tableUserData.EmailColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableUserData.EmailColumn] = value;
                }
            }
            
            public string ImageFileName {
                get {
                    try {
                        return ((string)(this[this.tableUserData.ImageFileNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableUserData.ImageFileNameColumn] = value;
                }
            }
            
            public bool IsUserIDNull() {
                return this.IsNull(this.tableUserData.UserIDColumn);
            }
            
            public void SetUserIDNull() {
                this[this.tableUserData.UserIDColumn] = System.Convert.DBNull;
            }
            
            public bool IsDisplayNameNull() {
                return this.IsNull(this.tableUserData.DisplayNameColumn);
            }
            
            public void SetDisplayNameNull() {
                this[this.tableUserData.DisplayNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsEmailNull() {
                return this.IsNull(this.tableUserData.EmailColumn);
            }
            
            public void SetEmailNull() {
                this[this.tableUserData.EmailColumn] = System.Convert.DBNull;
            }
            
            public bool IsImageFileNameNull() {
                return this.IsNull(this.tableUserData.ImageFileNameColumn);
            }
            
            public void SetImageFileNameNull() {
                this[this.tableUserData.ImageFileNameColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class UserDataRowChangeEvent : EventArgs {
            
            private UserDataRow eventRow;
            
            private DataRowAction eventAction;
            
            public UserDataRowChangeEvent(UserDataRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public UserDataRow Row {
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