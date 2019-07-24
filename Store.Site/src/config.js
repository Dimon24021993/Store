const config = {
    get baseApiUrl() {
        switch (window.location.hostname.toLowerCase()) {
            case "localhost":
                return "https://localhost:44375/api";
            case "mymoovies.ga":
                return "https://api.mymoovies.ga/api";
            default:
                return "";
        }
    },
}

export default config;