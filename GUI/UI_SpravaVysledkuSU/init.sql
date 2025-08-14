-- Experimenty
CREATE TABLE experiment (
  id BIGINT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(255) NOT NULL UNIQUE,
  description TEXT,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Modely
CREATE TABLE model (
  id BIGINT AUTO_INCREMENT PRIMARY KEY,
  model_type ENUM('RandomForestClassifier', 'SVC') NOT NULL,
  name VARCHAR(255) NOT NULL,
  description TEXT,
  params_json JSON NOT NULL,
  artifact_uri VARCHAR(1024),
  artifact_blob LONGBLOB,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT uq_model UNIQUE (model_type, name)
);

-- Vazba experiment <-> model
CREATE TABLE experiment_model (
  id BIGINT AUTO_INCREMENT PRIMARY KEY,
  experiment_id BIGINT NOT NULL,
  model_id BIGINT NOT NULL,
  assigned_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (experiment_id) REFERENCES experiment(id) ON DELETE CASCADE,
  FOREIGN KEY (model_id)      REFERENCES model(id)      ON DELETE CASCADE,
  UNIQUE (experiment_id, model_id)
);

-- Výsledky
CREATE TABLE result (
  id BIGINT AUTO_INCREMENT PRIMARY KEY,
  experiment_model_id BIGINT NOT NULL,
  metric_name VARCHAR(64) NOT NULL DEFAULT 'score',
  metric_value DOUBLE NOT NULL,
  created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (experiment_model_id) REFERENCES experiment_model(id) ON DELETE CASCADE
);

-- Indexy
CREATE INDEX ix_model_type ON model(model_type);
CREATE INDEX ix_result_value ON result(metric_value);
