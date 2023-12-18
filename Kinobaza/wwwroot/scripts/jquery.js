/* input file jquery*/

$(document).ready(function () {
   $(".input-file").change(function () {
      var f_name = [];
      for (var i = 0; i < $(this).get(0).files.length; ++i) {
         f_name.push($(this).get(0).files[i].name);
      }
      $("#inputField").val(f_name.join(", "));
   });
});