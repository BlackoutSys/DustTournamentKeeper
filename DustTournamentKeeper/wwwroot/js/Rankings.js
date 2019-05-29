function applyFilter(event) {
    var link = $("#applyFilter")[0];

    //link.href = link.href.replace("dateFrom=null", "dateFrom=" + $('#countryFilter').val());
    //link.href = link.href.replace("dateTo=null", "dateTo=" + $('#cityFilter').val());
    link.href = link.href.replace("country=null", "country=" + $('#countryFilter').val());
    link.href = link.href.replace("city=null", "city=" + $('#cityFilter').val());
    link.href = link.href.replace("clubId=null", "clubId=" + $('#clubIdFilter').val());
    link.href = link.href.replace("clubName=null", "clubName=" + $('#clubFilter').val());
    link.href = link.href.replace("bigPointsMin=null", "bigPointsMin=" + $('#bigPointsMinFilter').val());
    link.href = link.href.replace("bigPointsMax=null", "bigPointsMax=" + $('#bigPointsMaxFilter').val());
    link.href = link.href.replace("smallPointsMin=null", "smallPointsMin=" + $('#smallPointsMinFilter').val());
    link.href = link.href.replace("smallPointsMax=null", "smallPointsMax=" + $('#smallPointsMaxFilter').val());
}