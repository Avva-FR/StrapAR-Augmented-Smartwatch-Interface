var activeSectionIdentifier = "0";

(function() {
	//section Identifiere
	var sectionIdentifiers = [0, 1, 2, 3];
	var self = this,
		page = document.getElementById( "pageIndicatorPage" ),
		changer = document.getElementById( "hsectionchanger" ),
		sectionChanger,
		elPageIndicator = document.getElementById("pageIndicator"),
		pageIndicator,
		pageIndicatorHandler;
	
		var red_state = 0;
		var yellow_state = 0;
	

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
		pageIndicator.setActive(e.detail.active);
		
		activeSectionIdentifier = sectionIdentifiers[e.detail.active];
		sendMessage(activeSectionIdentifier);
		//log = activeSectionIdentifier;
		//aktualisiereAnzeige();
	};
	
	// Bind the callback
	changer.addEventListener("sectionchange", pageIndicatorHandler, false);
}());


