(function() {
	//section Identifiere
	var sectionIdentifiers = ['home', 'weather', 'diagramm', 'document'];
	var self = this,
		page = document.getElementById( "pageIndicatorPage" ),
		changer = document.getElementById( "hsectionchanger" ),
		sectionChanger,
		elPageIndicator = document.getElementById("pageIndicator"),
		pageIndicator,
		pageIndicatorHandler;
		var red_state = 0;
		var yellow_state = 0;
		document.getElementById("1").addEventListener("click", changeBGYellow);
		document.getElementById("2").addEventListener("click", changeBGRed);

	page.addEventListener( "pagebeforeshow", function() {
		// Create PageIndicator
		pageIndicator =  tau.widget.PageIndicator(elPageIndicator, { numberOfPages: 4 });
		pageIndicator.setActive(0);

		sectionChanger = new tau.widget.SectionChanger(changer, {
			circular: true,
			orientation: "horizontal",
			useBouncingEffect: true
		});
	});

	page.addEventListener( "pagehide", function() {
		sectionChanger.destroy();
		pageIndicator.destroy();
	});

	// Indicator setting handler
	pageIndicatorHandler = function (e) {
		removeBGChanges();
		pageIndicator.setActive(e.detail.active);
		
		var activeSectionIdentifier = sectionIdentifiers[e.detail.active];
		sendMessage(activeSectionIdentifier);
		//log = activeSectionIdentifier;
		aktualisiereAnzeige();
	};
	
	// Bind the callback
	changer.addEventListener("sectionchange", pageIndicatorHandler, false);
	
	function changeBGRed(){
		if(red_state == 0){
			document.getElementById("stock").style.backgroundColor = "#400000";
			sendMessage("1");
			red_state = 1;
		} else{
			document.getElementById("stock").style.backgroundColor = "#000000";
			red_state = 0;
		}
	}
	
	function changeBGYellow(){
		if(yellow_state == 0){
			document.getElementById("weather").style.backgroundColor = "#525100";
			sendMessage("2");
			yellow_state = 1;
		} else{
			document.getElementById("weather").style.backgroundColor = "#000000";
			yellow_state = 0;
		}
	}
	
	function removeBGChanges(){
		red_state= 0;
		yellow_state= 0;
		document.getElementById("stock").style.backgroundColor = "#000000";
		document.getElementById("weather").style.backgroundColor = "#000000";
	}
}());





