class User {
    constructor() {
        this.role = "";
        this.id = "";
        this.username = "";
        this.isAuthenticated = false;
    }

    setUser(data) {
        if (!data) {
            console.warn("setUser: No data provided");
            return;
        }
        
        this.role = data.role || "";
        this.id = data.id || data.user_id || "";
        this.username = data.username || "";
        this.email = data.email || "";
        this.isAuthenticated = true;
    }

    hasRole(role) {
        return this.role === role;
    }

    isSpravce() {
        return this.hasRole("spravce");
    }

    isSkladnik() {
        return this.hasRole("skladnik");
    }

    // Check if user has any data
    hasData() {
        return this.isAuthenticated && this.id && this.role;
    }

    // Clear user data
    clear() {
        this.role = "";
        this.id = "";
        this.username = "";
        this.email = "";
        this.isAuthenticated = false;
    }
}

export default User;