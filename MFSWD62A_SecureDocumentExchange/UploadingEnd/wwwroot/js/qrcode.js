﻿attachEvent = (target, event, callback) => target.addEventListener(event, callback);

attachEvent(window, "load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
});

