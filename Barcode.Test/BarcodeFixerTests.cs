using Xunit;

namespace Barcode.Test;

public class BarcodeFixerTests
{
    [Fact]
    public void FixBarcode_ValidBarcodeWithOneBrokenSymbol_ReturnsFixedBarcode()
    {
        // Arrange
        var barcode = "40063X1333931";
        var expected = "4006381333931";

        // Act
        var result = BarcodeFixer.FixBarcode(barcode);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FixBarcode_ValidBarcodeNoBrokenSymbol_ReturnsOriginalBarcode()
    {
        // Arrange
        var barcode = "4006313339317"; // A valid barcode with no broken symbol

        // Act
        var result = BarcodeFixer.FixBarcode(barcode);

        // Assert
        Assert.Equal(barcode, result);
    }

    [Fact]
    public void FixBarcode_InvalidLength_ThrowsArgumentException()
    {
        // Arrange
        var barcode = "400638133393"; // Less than 13 characters

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => BarcodeFixer.FixBarcode(barcode));
        Assert.Contains("Invalid barcode length", exception.Message);
    }

    [Fact]
    public void FixBarcode_MoreThanOneBrokenSymbol_ThrowsArgumentException()
    {
        // Arrange
        var barcode = "40063X13339X1"; // Two 'X' characters

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => BarcodeFixer.FixBarcode(barcode));
        Assert.Contains("Barcode must contain only one broken symbol", exception.Message);
    }

    [Fact]
    public void FixBarcode_InvalidCharacters_ThrowsArgumentException()
    {
        // Arrange
        var barcode = "40063A1333931"; // Non-numeric character 'A'

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => BarcodeFixer.FixBarcode(barcode));
        Assert.Contains("Barcode contains non-numeric characters", exception.Message);
    }
}