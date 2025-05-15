attachEvent = (event, target, callback) => {
    target.addEventListener(event, callback);
}

attachEvent("click", document.getElementById("download"), () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
    {
        text: uri,
        width: 150,
        height: 150
    });
});

