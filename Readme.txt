To compile and run Schematron.NET as well as the samples:

1 - Create a virtual directory in IIS pointing to \samples\SchematronWS with the name SchematronWS.
2 - Open the NMatrix.Schematron+Samples.sln file.
3 - Compile the solution.

You can run the client WinForms app, which contains a UI for testing the product. Tips:

- If you click Execute, the schema and XML document will be dumped to the appropriate tabs, so you see which was the input for validation.
- If you leave the XML File field blank, the text in the Document will be used instead, as XML. Beware that no fancy exception handling is in-place.
- To change the errors output formatting in the Execute button, select the one you want from the dropdown.
- If you click on the Send 2 WS button, the file in the XML File field (or the Document tab if it's left blank) will be sent to the testing webservice. 
- To change the errors output formatting in the webservice, you'll need to modify the ValidationAttribute on the \samples\SchematronWS\ValidatedWS.asmx.cs file, for the webmethod. It looks like the following:

	[WebMethod]
	[Validation("po-schema.xsd", OutputFormatting.Log)]
	public void BatchInsert(XmlNode orders)
