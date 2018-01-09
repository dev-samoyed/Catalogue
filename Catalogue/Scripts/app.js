//on close collapse
$('.collapse').on('hidden.bs.collapse', function () {
  var target = '#'+$(this).attr('data-parent');
  $(target).removeClass('collapse-open');
});

//on open collapse
$('.collapse').on('shown.bs.collapse', function () {
  var target = '#'+$(this).attr('data-parent');
  $(target).addClass('collapse-open');
})
