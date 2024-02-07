function change_Page() {
	//log = "PageChange";
	//aktualisiereAnzeige();
    if (activeSectionIdentifier == "1") {
    		sendMessage("a1");
        window.location.href = 'weather.html';
    } else if (activeSectionIdentifier == "2") {
    		sendMessage("a2");
        window.location.href = 'stock.html';
    } else if (activeSectionIdentifier == "3") {
    		sendMessage("a3");
        window.location.href = 'document.html';
    }
}


function back_home() {
	//log = "back";
	//aktualisiereAnzeige();
	window.location.href = 'index.html';
}
