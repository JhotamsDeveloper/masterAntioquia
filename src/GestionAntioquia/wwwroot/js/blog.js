/* JS Document */

/******************************

[Table of Contents]

1. Init Isotope


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
		if($('.blog_posts').length)
		{
			var grid = $('.blog_posts');
			grid.isotope(
			{
				itemSelector:'.blog_post'
			});
		}
	}

});