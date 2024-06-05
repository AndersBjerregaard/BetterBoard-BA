namespace WebDriverXUnit.Assertions.Interfaces;

public interface IUnsignedDocumentsAssertion {
    // IUnsignedDocumentsAssertion HasDocumentFromOrigin(string origin);
    IUnsignedDocumentsAssertion HasDocumentFromOrigin(params string[] origins);
    IUnsignedDocumentsAssertion HasNoDocumentFromorigin(string origin);
}