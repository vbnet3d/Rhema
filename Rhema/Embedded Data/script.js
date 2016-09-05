$(".word")
	.mouseover(function () {
		$(this).css("background-color", "yellow");
		$("#info").html($(this).attr('id'));
	})
	.mouseleave(function() {
		$( this ).css("background-color", "white");
	});