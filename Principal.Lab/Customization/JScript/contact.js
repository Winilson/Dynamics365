function ValidarMetodoPreferencialdeComunicacao(contexto) {

    var ContextoCrm = contexto.getFormContext();
    
    var prefferedContactMethodCode =
        ContextoCrm.getAttribute('preferredcontactmethodcode').getValue();

    if (prefferedContactMethodCode == 1) {
        clearAllMandatoryFields(contexto);
    }
    if (prefferedContactMethodCode == 2) {
        clearAllMandatoryFields(contexto);
        ContextoCrm.getAttribute('emailaddress1').setRequiredLevel('required');
    } else if (prefferedContactMethodCode == 3) {
        clearAllMandatoryFields(contexto);
        ContextoCrm.getAttribute('mobilephone').setRequiredLevel('required');
    } else if (prefferedContactMethodCode == 4) {
        clearAllMandatoryFields(contexto);
        ContextoCrm.getAttribute('fax').setRequiredLevel('required');
    }
}
function clearAllMandatoryFields(contexto) {

    var contextoCrm = contexto.getFormContext();
   
    contextoCrm.getAttribute('emailaddress1').setRequiredLevel('none');
    contextoCrm.getAttribute('mobilephone').setRequiredLevel('none');
    contextoCrm.getAttribute('fax').setRequiredLevel('none');
}