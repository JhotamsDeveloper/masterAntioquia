/* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init Accordions
5. Init Gallery



******************************/

$(document).ready(function () {
	"use strict";

	/* 

	1. Vars and Inits

	*/

	var header = $('.header');
	var map;

	setHeader();

	$(window).on('resize', function () {
		setHeader();

		setTimeout(function () {
			$(window).trigger('resize.px.parallax');
		}, 375);
	});

	$(document).on('scroll', function () {
		setHeader();
	});

	initMenu();
	initAccordions();
	initGallery();

	/* 

	2. Set Header

	*/

	function setHeader() {
		if ($(window).scrollTop() > 91) {
			header.addClass('scrolled');
		}
		else {
			header.removeClass('scrolled');
		}
	}

	/* 

	3. Init Menu

	*/

	function initMenu() {
		if ($('.menu').length) {
			var header = $('.header');
			var hOverlay = $('.header_overlay');
			var menu = $('.menu');
			var hamb = $('.hamburger');
			var sup = $('.super_container_inner');
			var close = $('.menu_close');
			var overlay = $('.super_overlay');

			hamb.on('click', function () {
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});

			close.on('click', function () {
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});

			overlay.on('click', function () {
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});

			hOverlay.on('click', function () {
				header.toggleClass('active');
				sup.toggleClass('active');
				menu.toggleClass('active');
			});
		}
	}

	/* 

	4. Init Accordions

	*/

	function initAccordions() {
		if ($('.accordion').length) {
			var accs = $('.accordion');

			accs.each(function () {
				var acc = $(this);

				if (acc.hasClass('active')) {
					var panel = $(acc.next());
					var panelH = panel.prop('scrollHeight') + "px";

					if (panel.css('max-height') == "0px") {
						panel.css('max-height', panel.prop('scrollHeight') + "px");
					}
					else {
						panel.css('max-height', "0px");
					}
				}

				acc.on('click', function () {
					if (acc.hasClass('active')) {
						acc.removeClass('active');
						var panel = $(acc.next());
						var panelH = panel.prop('scrollHeight') + "px";

						if (panel.css('max-height') == "0px") {
							panel.css('max-height', panel.prop('scrollHeight') + "px");
						}
						else {
							panel.css('max-height', "0px");
						}
					}
					else {
						acc.addClass('active');
						var panel = $(acc.next());
						var panelH = panel.prop('scrollHeight') + "px";

						if (panel.css('max-height') == "0px") {
							panel.css('max-height', panel.prop('scrollHeight') + "px");
						}
						else {
							panel.css('max-height', "0px");
						}
					}
				});
			});
		}
	}

	/* 

	5. Init Gallery

	*/

	function initGallery() {
		if ($('.review_image').length) {
			$('.galleriesTour').colorbox(
				{
					rel: 'colorbox',
					photo: true,
					opacity: 0.85,
					maxWidth: '95%',
					maxHeight: '95%'
				});
		}
	}
});