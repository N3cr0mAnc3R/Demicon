window.onload = function () {
    const app = new Vue({
        el: "#app",
        data: {
            countryNames: [],
            countries: [],
            currentCountry: '',
            users: [],
        },
        methods: {
            init: function () {
                let self = this;
                self.loadData(a => {
                    let response = a;
                    self.countryNames = response.map(a => { return a.name; });
                    self.currentCountry = self.countryNames[0];
                    self.countries = response;
                });
            },
            loadData: function (callback) {
                let req = new XMLHttpRequest();
                req.open("get", "/api/user");
                req.onload = function () {
                    callback(JSON.parse(req.response));
                }
                req.send();
            },
            reload: function () {
                let self = this;
                self.loadData(a => {
                    let response = a;
                    self.countryNames = response.map(a => { return a.name; });
                    self.countries = response;
                });
            }
        },
        mounted: function () {
            this.init();
        },
        watch: {
            currentCountry: function (newV, oldV) {
                let self = this;
                self.users = self.countries.find(a => a.name == newV).users;
            }
        }
    });
};