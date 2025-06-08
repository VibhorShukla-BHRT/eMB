//On UpdatePanel Refresh.
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (prm != null) {
                    prm.add_endRequest(function (sender, e) {
                        if (sender._postBackSettings.panelsToUpdate != null) {
                          utility(); 
                        }
                    });
                };
    $( document ).ready(function() { 
        utility();
    });
    function utility() {
          $(".datepicker").datepicker({
              changeMonth: true,
              changeYear: true,
              yearRange: '1950:' + new Date().getFullYear() + '',
              dateFormat: 'dd-mm-yy'
          });

        $(".Stoptyping").on("keypress keyup blur", function (event) {
            event.preventDefault();
        });

        $(".Decimal").on("keypress keyup blur", function (event) {
            $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
            if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });
        
 
        $(".Numbers").on("keypress keyup blur", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });

    }
     function validateEmail(email) {
        const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    } 
    function ShowPopup() {  
        $("#btnModal").click();
    }
    function ClosePopup() {
        $('#myModal').modal('hide');
        $('.modal-backdrop').hide();
    }