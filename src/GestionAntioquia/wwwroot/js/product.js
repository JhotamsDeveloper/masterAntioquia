(function ($) {

	"use strict";

	$('.appointment_date-check-in').datepicker({
		'format': 'm/d/yyyy',
		'autoclose': true
	});
	$('.appointment_date-check-out').datepicker({
		'format': 'm/d/yyyy',
		'autoclose': true
	});

	$('.appointment_time').timepicker();



})(jQuery);