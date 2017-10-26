angular.module('CarFinderApp')
    .controller('CarFinderAppController', ['$scope', '$http', function ($scope, $http) {

        /* Strings */
        $scope.years = null;
        $scope.makes = null;
        $scope.models = null;
        $scope.trims = null;

        //Declare empty strings
        $scope.selectedYear = "";
        $scope.selectedMake = "";
        $scope.selectedModel = "";
        $scope.selectedTrim = "";
        $scope.carData = null;

        $scope.showYearLoadingMessage = false;
        $scope.showMakeLoadingMessage = false;
        $scope.showModelLoadingMessage = false;
        $scope.showTrimLoadingMessage = false;
        $scope.showCarDataLoadingMessage = false;

        $scope.myInterval = 4000;
        $scope.wrapSliders = 4000;

        $scope.isCollapsed = true;
        $scope.isCollapsedTwo = true;

        // we use {} cause carData is a object, it holds objects.  Return one object, years,makes,models,trims
        

        $scope.getYears = function () {
            

            $scope.showYearLoadingMessage = true;
            $scope.showMakeLoadingMessage = false;
            $scope.showModelLoadingMessage = false;
            $scope.showTrimLoadingMessage = false;
            $scope.makes = null;
            $scope.models = null;
            $scope.trims = null;
            $scope.carData = null;

             $http.get('api/Cars/Years').then(function (response) {
                $scope.years = response.data; 
                $scope.showYearLoadingMessage = false;
            });

        };
        $scope.getMakes = function () {
           
            $scope.showMakeLoadingMessage = true;
            $scope.showModelLoadingMessage = false;
            $scope.showTrimLoadingMessage = false;
            $scope.models = null;
            $scope.trims = null;
            $scope.carData = null;

            var options = {
                params: {
                    year: $scope.selectedYear
                }
            }; //pass the selected year to the API

            $http.get('api/Cars/Makes', options).then(function (response) {
                $scope.makes = response.data;
                $scope.showMakeLoadingMessage = false;
            });

            };

        $scope.getModels = function () {
            if ($scope.selectedMake != null) {

                $scope.showModelLoadingMessage = true;
                $scope.showTrimLoadingMessage = false;
                $scope.showCarDataLoadingMessage = false;
                $scope.models = null;
                $scope.trims = null;
                $scope.carData = null;

                var options = {
                    params: {
                        year: $scope.selectedYear,
                        make: $scope.selectedMake
                    }
                }; //pass the selected year to the API

                $http.get('api/Cars/Models', options).then(function (response) {
                    $scope.models = response.data;
                    $scope.showModelLoadingMessage = false;
                });
            }
        };
        $scope.getTrims = function () {
            if ($scope.selectedModel != null)
            { 
                $scope.showTrimLoadingMessage = true;
                $scope.trims = null;
                $scope.carData = null;

                var options = {
                    params: {
                        year: $scope.selectedYear,
                        make: $scope.selectedMake,
                        model: $scope.selectedModel,
                        trim: $scope.selectedTrim
                    }
                };//pass the selected year to the API

                $http.get('api/Cars/Trims', options).then(function (response) {
                    $scope.trims = response.data;
                    $scope.showTrimLoadingMessage = false;

                    if ($scope.trims[0] === "" && $scope.trims.length == 1) {
                        $scope.getCar();
                    }
                });
            }
      
        };

        $scope.getCar = function () {

            if ($scope.selectedModel != null) {

                $scope.carData = null;
                $scope.showCarDataLoadingMessage = true;

                var options = {
                    params: {
                        year: $scope.selectedYear,
                        make: $scope.selectedMake,
                        model: $scope.selectedModel
                    }
                };

                if ($scope.selectedTrim != "") {
                    options.params.trim = $scope.selectedTrim;
                }

                $http.get('api/Cars/Cars', options).then(function (response) {
                    $scope.carData = response.data;

                    for (var property in $scope.carData.cars) {
                        if ($scope.carData.cars[property].model_trim === "" ||
                            $scope.carData.cars[property].model_trim === "Not Available") {
                            $scope.carData.cars[property].model_trim = "";
                        }
                    }
                    $scope.showCarDataLoadingMessage = false;
                });
            }
        };

        $scope.getYears();

    }]);

