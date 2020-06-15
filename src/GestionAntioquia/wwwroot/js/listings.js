/* JS Document */

/******************************

[Table of Contents]

4. Init Isotope


******************************/

$(document).ready(function()
{
	"use strict";

	initIsotope();

	/*

	4. Init Isotope

	*/

	function initIsotope()
	{
		if($('.grid').length)
		{
			var grid = $('.grid');
			grid.isotope(
			{
				itemSelector:'.grid-item',
				layoutMode: 'fitRows'
			});

			// Filtering
			var checkboxes =  $('.listing_checkbox label input');
	        checkboxes.on('click', function()
	        {
	        	var checked = checkboxes.filter(':checked');
	        	var filters = [];
	        	checked.each(function()
	        	{
	        		var filterValue = $(this).attr('data-filter');
	        		filters.push(filterValue);
	        	});

	        	filters = filters.join(', ');
	        	grid.isotope({filter: filters});
	        });
		}
	}



});