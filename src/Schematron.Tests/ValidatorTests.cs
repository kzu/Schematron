using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using UnitTesting = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Schematron.Tests
{
    [TestClass]
    public class ValidatorTests
    {
        const string XsdLocation = "./Content/po-schema.xsd";
        const string XmlContentLocation = "./Content/po-instance.xml";
        const string TargetNamespace = "http://example.com/po-schematron";

        public ValidatorTests()
        {

        }

        [ExpectedException(typeof(Schematron.ValidationException))]
        [TestMethod]
        public void NewAddSchemaSignatureShouldNotBreakCode()
        {
            var validatorA = new Validator(OutputFormatting.XML);
            validatorA.AddSchema(XmlReader.Create(XsdLocation));

            var validatorB = new Validator(OutputFormatting.XML);
            validatorB.AddSchema(TargetNamespace, XsdLocation);

            var resultA = default(string);
            var resultB = default(string);

            try
            {
                var result = validatorA.Validate(XmlReader.Create(XmlContentLocation));
            }
            catch (ValidationException ex)
            {
                resultA = ex.Message;

                System.Diagnostics.Debug.WriteLine(ex.Message);                
            }

            try
            {
                var result = validatorB.Validate(XmlReader.Create(XmlContentLocation));
            }
            catch (ValidationException ex)
            {
                resultB = ex.Message;

                UnitTesting.Assert.IsTrue(resultA == resultB);

                System.Diagnostics.Debug.WriteLine(ex.Message);

                throw;
            }

        }

        [TestMethod]
        public void ValidateShouldReturnSchematronValidationResultWhenSchematronConstraintsAreNotMet()
        {
            //Arrange
            var validator = new Validator(OutputFormatting.XML);

            //Act
            validator.AddSchema(TargetNamespace, XsdLocation);

            using (var doc = XmlReader.Create(XmlContentLocation))
            {
                var result = default(IXPathNavigable);

                try
                {
                    result = validator.Validate(doc);
                }
                catch (ValidationException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);

                    var serializer = new XmlSerializer(typeof(Schematron.Serialization.SchematronValidationResultTempObjectModel.output));

                    using (var stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(ex.Message)))
                    using (var reader = XmlReader.Create(stream))
                    {
                        var obj = (Schematron.Serialization.SchematronValidationResultTempObjectModel.output)serializer.Deserialize(reader);

                        // Assert

                        UnitTesting.Assert.IsNotNull(obj);
                        UnitTesting.Assert.IsNotNull(obj.xml);
                        UnitTesting.Assert.IsNotNull(obj.schematron);
                    }
                }
            }
        }
    }
}
