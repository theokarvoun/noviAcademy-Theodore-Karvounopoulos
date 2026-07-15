namespace NoviCode.Gateway;



    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.gesmes.org/xml/2002-08-01")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.gesmes.org/xml/2002-08-01", IsNullable = false)]
    public partial class ResponseDto
    {

        private string subjectField;

        private EnvelopeSender senderField;

        private Cube cubeField;

        /// <remarks/>
        public string subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        public EnvelopeSender Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref")]
        public Cube Cube
        {
            get
            {
                return this.cubeField;
            }
            set
            {
                this.cubeField = value;
            }
        }
    }


