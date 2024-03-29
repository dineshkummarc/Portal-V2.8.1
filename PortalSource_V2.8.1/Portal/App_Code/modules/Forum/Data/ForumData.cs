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
    public class ForumData : DataSet {
        
        private ForumDataTable tableForum;
        
        public ForumData() {
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            this.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        protected ForumData(SerializationInfo info, StreamingContext context) {
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((strSchema != null)) {
                DataSet ds = new DataSet();
                ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["Forum"] != null)) {
                    this.Tables.Add(new ForumDataTable(ds.Tables["Forum"]));
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
        public ForumDataTable Forum {
            get {
                return this.tableForum;
            }
        }
        
        public override DataSet Clone() {
            ForumData cln = ((ForumData)(base.Clone()));
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
            if ((ds.Tables["Forum"] != null)) {
                this.Tables.Add(new ForumDataTable(ds.Tables["Forum"]));
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
            this.tableForum = ((ForumDataTable)(this.Tables["Forum"]));
            if ((this.tableForum != null)) {
                this.tableForum.InitVars();
            }
        }
        
        private void InitClass() {
            this.DataSetName = "ForumData";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/ForumData.xsd";
            this.Locale = new System.Globalization.CultureInfo("en-US");
            this.CaseSensitive = false;
            this.EnforceConstraints = true;
            this.tableForum = new ForumDataTable();
            this.Tables.Add(this.tableForum);
        }
        
        private bool ShouldSerializeForum() {
            return false;
        }
        
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        public delegate void ForumRowChangeEventHandler(object sender, ForumRowChangeEvent e);
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ForumDataTable : DataTable, System.Collections.IEnumerable {
            
            private DataColumn columnThreadFile;
            
            private DataColumn columnDateTime;
            
            private DataColumn columnUserId;
            
            private DataColumn columnAuthor;
            
            private DataColumn columnEmail;
            
            private DataColumn columnTitle;
            
            private DataColumn columnText;
            
            private DataColumn columnCommentCount;
            
            private DataColumn columnLastPosterName;
            
            private DataColumn columnLastPosterId;
            
            private DataColumn columnId;
            
            public ForumDataTable() : 
                    base("Forum") {
                this.InitClass();
            }
            
            public ForumDataTable(DataTable table) : 
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
            
            public DataColumn ThreadFileColumn {
                get {
                    return this.columnThreadFile;
                }
            }
            
            public DataColumn DateTimeColumn {
                get {
                    return this.columnDateTime;
                }
            }
            
            public DataColumn UserIdColumn {
                get {
                    return this.columnUserId;
                }
            }
            
            public DataColumn AuthorColumn {
                get {
                    return this.columnAuthor;
                }
            }
            
            public DataColumn EmailColumn {
                get {
                    return this.columnEmail;
                }
            }
            
            public DataColumn TitleColumn {
                get {
                    return this.columnTitle;
                }
            }
            
            public DataColumn TextColumn {
                get {
                    return this.columnText;
                }
            }
            
            public DataColumn CommentCountColumn {
                get {
                    return this.columnCommentCount;
                }
            }
            
            public DataColumn LastPosterNameColumn {
                get {
                    return this.columnLastPosterName;
                }
            }
            
            public DataColumn LastPosterIdColumn {
                get {
                    return this.columnLastPosterId;
                }
            }
            
            public DataColumn IdColumn {
                get {
                    return this.columnId;
                }
            }
            
            public ForumRow this[int index] {
                get {
                    return ((ForumRow)(this.Rows[index]));
                }
            }
            
            public event ForumRowChangeEventHandler ForumRowChanged;
            
            public event ForumRowChangeEventHandler ForumRowChanging;
            
            public event ForumRowChangeEventHandler ForumRowDeleted;
            
            public event ForumRowChangeEventHandler ForumRowDeleting;
            
            public void AddForumRow(ForumRow row) {
                this.Rows.Add(row);
            }
            
            public ForumRow AddForumRow(string ThreadFile, System.DateTime DateTime, string UserId, string Author, string Email, string Title, string Text, int CommentCount, string LastPosterName, string LastPosterId) {
                ForumRow rowForumRow = ((ForumRow)(this.NewRow()));
                rowForumRow.ItemArray = new object[] {
                        ThreadFile,
                        DateTime,
                        UserId,
                        Author,
                        Email,
                        Title,
                        Text,
                        CommentCount,
                        LastPosterName,
                        LastPosterId,
                        null};
                this.Rows.Add(rowForumRow);
                return rowForumRow;
            }
            
            public System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            public override DataTable Clone() {
                ForumDataTable cln = ((ForumDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            protected override DataTable CreateInstance() {
                return new ForumDataTable();
            }
            
            public void InitVars() {
                this.columnThreadFile = this.Columns["ThreadFile"];
                this.columnDateTime = this.Columns["DateTime"];
                this.columnUserId = this.Columns["UserId"];
                this.columnAuthor = this.Columns["Author"];
                this.columnEmail = this.Columns["Email"];
                this.columnTitle = this.Columns["Title"];
                this.columnText = this.Columns["Text"];
                this.columnCommentCount = this.Columns["CommentCount"];
                this.columnLastPosterName = this.Columns["LastPosterName"];
                this.columnLastPosterId = this.Columns["LastPosterId"];
                this.columnId = this.Columns["Id"];
            }
            
            private void InitClass() {
                this.columnThreadFile = new DataColumn("ThreadFile", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnThreadFile);
                this.columnDateTime = new DataColumn("DateTime", typeof(System.DateTime), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnDateTime);
                this.columnUserId = new DataColumn("UserId", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnUserId);
                this.columnAuthor = new DataColumn("Author", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnAuthor);
                this.columnEmail = new DataColumn("Email", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnEmail);
                this.columnTitle = new DataColumn("Title", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnTitle);
                this.columnText = new DataColumn("Text", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnText);
                this.columnCommentCount = new DataColumn("CommentCount", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnCommentCount);
                this.columnLastPosterName = new DataColumn("LastPosterName", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnLastPosterName);
                this.columnLastPosterId = new DataColumn("LastPosterId", typeof(string), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnLastPosterId);
                this.columnId = new DataColumn("Id", typeof(int), null, System.Data.MappingType.Element);
                this.Columns.Add(this.columnId);
                this.columnId.AutoIncrement = true;
            }
            
            public ForumRow NewForumRow() {
                return ((ForumRow)(this.NewRow()));
            }
            
            protected override DataRow NewRowFromBuilder(DataRowBuilder builder) {
                return new ForumRow(builder);
            }
            
            protected override System.Type GetRowType() {
                return typeof(ForumRow);
            }
            
            protected override void OnRowChanged(DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.ForumRowChanged != null)) {
                    this.ForumRowChanged(this, new ForumRowChangeEvent(((ForumRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowChanging(DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.ForumRowChanging != null)) {
                    this.ForumRowChanging(this, new ForumRowChangeEvent(((ForumRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleted(DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.ForumRowDeleted != null)) {
                    this.ForumRowDeleted(this, new ForumRowChangeEvent(((ForumRow)(e.Row)), e.Action));
                }
            }
            
            protected override void OnRowDeleting(DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.ForumRowDeleting != null)) {
                    this.ForumRowDeleting(this, new ForumRowChangeEvent(((ForumRow)(e.Row)), e.Action));
                }
            }
            
            public void RemoveForumRow(ForumRow row) {
                this.Rows.Remove(row);
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ForumRow : DataRow {
            
            private ForumDataTable tableForum;
            
            public ForumRow(DataRowBuilder rb) : 
                    base(rb) {
                this.tableForum = ((ForumDataTable)(this.Table));
            }
            
            public string ThreadFile {
                get {
                    try {
                        return ((string)(this[this.tableForum.ThreadFileColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.ThreadFileColumn] = value;
                }
            }
            
            public System.DateTime DateTime {
                get {
                    try {
                        return ((System.DateTime)(this[this.tableForum.DateTimeColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.DateTimeColumn] = value;
                }
            }
            
            public string UserId {
                get {
                    try {
                        return ((string)(this[this.tableForum.UserIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.UserIdColumn] = value;
                }
            }
            
            public string Author {
                get {
                    try {
                        return ((string)(this[this.tableForum.AuthorColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.AuthorColumn] = value;
                }
            }
            
            public string Email {
                get {
                    try {
                        return ((string)(this[this.tableForum.EmailColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.EmailColumn] = value;
                }
            }
            
            public string Title {
                get {
                    try {
                        return ((string)(this[this.tableForum.TitleColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.TitleColumn] = value;
                }
            }
            
            public string Text {
                get {
                    try {
                        return ((string)(this[this.tableForum.TextColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.TextColumn] = value;
                }
            }
            
            public int CommentCount {
                get {
                    try {
                        return ((int)(this[this.tableForum.CommentCountColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.CommentCountColumn] = value;
                }
            }
            
            public string LastPosterName {
                get {
                    try {
                        return ((string)(this[this.tableForum.LastPosterNameColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.LastPosterNameColumn] = value;
                }
            }
            
            public string LastPosterId {
                get {
                    try {
                        return ((string)(this[this.tableForum.LastPosterIdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.LastPosterIdColumn] = value;
                }
            }
            
            public int Id {
                get {
                    try {
                        return ((int)(this[this.tableForum.IdColumn]));
                    }
                    catch (InvalidCastException e) {
                        throw new StrongTypingException("Der Wert kann nicht ermittelt werden, da er DBNull ist.", e);
                    }
                }
                set {
                    this[this.tableForum.IdColumn] = value;
                }
            }
            
            public bool IsThreadFileNull() {
                return this.IsNull(this.tableForum.ThreadFileColumn);
            }
            
            public void SetThreadFileNull() {
                this[this.tableForum.ThreadFileColumn] = System.Convert.DBNull;
            }
            
            public bool IsDateTimeNull() {
                return this.IsNull(this.tableForum.DateTimeColumn);
            }
            
            public void SetDateTimeNull() {
                this[this.tableForum.DateTimeColumn] = System.Convert.DBNull;
            }
            
            public bool IsUserIdNull() {
                return this.IsNull(this.tableForum.UserIdColumn);
            }
            
            public void SetUserIdNull() {
                this[this.tableForum.UserIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsAuthorNull() {
                return this.IsNull(this.tableForum.AuthorColumn);
            }
            
            public void SetAuthorNull() {
                this[this.tableForum.AuthorColumn] = System.Convert.DBNull;
            }
            
            public bool IsEmailNull() {
                return this.IsNull(this.tableForum.EmailColumn);
            }
            
            public void SetEmailNull() {
                this[this.tableForum.EmailColumn] = System.Convert.DBNull;
            }
            
            public bool IsTitleNull() {
                return this.IsNull(this.tableForum.TitleColumn);
            }
            
            public void SetTitleNull() {
                this[this.tableForum.TitleColumn] = System.Convert.DBNull;
            }
            
            public bool IsTextNull() {
                return this.IsNull(this.tableForum.TextColumn);
            }
            
            public void SetTextNull() {
                this[this.tableForum.TextColumn] = System.Convert.DBNull;
            }
            
            public bool IsCommentCountNull() {
                return this.IsNull(this.tableForum.CommentCountColumn);
            }
            
            public void SetCommentCountNull() {
                this[this.tableForum.CommentCountColumn] = System.Convert.DBNull;
            }
            
            public bool IsLastPosterNameNull() {
                return this.IsNull(this.tableForum.LastPosterNameColumn);
            }
            
            public void SetLastPosterNameNull() {
                this[this.tableForum.LastPosterNameColumn] = System.Convert.DBNull;
            }
            
            public bool IsLastPosterIdNull() {
                return this.IsNull(this.tableForum.LastPosterIdColumn);
            }
            
            public void SetLastPosterIdNull() {
                this[this.tableForum.LastPosterIdColumn] = System.Convert.DBNull;
            }
            
            public bool IsIdNull() {
                return this.IsNull(this.tableForum.IdColumn);
            }
            
            public void SetIdNull() {
                this[this.tableForum.IdColumn] = System.Convert.DBNull;
            }
        }
        
        [System.Diagnostics.DebuggerStepThrough()]
        public class ForumRowChangeEvent : EventArgs {
            
            private ForumRow eventRow;
            
            private DataRowAction eventAction;
            
            public ForumRowChangeEvent(ForumRow row, DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            public ForumRow Row {
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
