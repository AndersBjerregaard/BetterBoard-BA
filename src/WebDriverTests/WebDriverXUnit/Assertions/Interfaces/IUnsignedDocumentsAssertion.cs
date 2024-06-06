namespace WebDriverXUnit.Assertions.Interfaces;

public interface IUnsignedDocumentsAssertion {
    IUnsignedDocumentsAssertion HasDocumentFromOrigin(params string[] origins);
    IUnsignedDocumentsAssertion HasNoDocumentFromorigin(string origin);
}